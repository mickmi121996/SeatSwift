using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.ViewModels;
using GuichetAutonome.ViewModels.Pages;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for SeatSelection.xaml
    /// </summary>
    public partial class SeatSelection : Page
    {
        public SeatSelection(Representation representation, int numberOfTicket)
        {
            InitializeComponent();
            VMSeatSelection vMSeatSelection = new VMSeatSelection(representation, numberOfTicket);
            this.DataContext = vMSeatSelection;

            // S'abonner à l'événement SeatsInitialized
            vMSeatSelection.SeatsInitialized += ViewModel_SeatsInitialized;

            vMSeatSelection.FilterOrSectionChanged += async () =>
            {
                await SelectBestAdjacentSeats(vMSeatSelection.NumberOfTickets, vMSeatSelection.SelectedFilter);
            };

            // Attacher le gestionnaire d'événements Unloaded
            this.Unloaded += ManageTheater_Unloaded;
        }

        private void ViewModel_SeatsInitialized()
        {
            // Appeler ConfigureGrid sur le thread UI
            Dispatcher.Invoke(() =>
            {
                ConfigureGrid();
            });
        }

        private void ManageTheater_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is VMSeatSelection viewModel)
            {
                viewModel.SeatsInitialized -= ViewModel_SeatsInitialized;
            }

            this.Unloaded -= ManageTheater_Unloaded;
        }


        public async void ConfigureGrid()
        {
            var viewModel = DataContext as VMSeatSelection;
            if (viewModel == null || viewModel.Auditorium == null) return;

            var rows = viewModel.Auditorium.NumberOfRows;
            var columns = viewModel.Auditorium.NumberOfColumns;

            // Accès direct au Grid défini dans le XAML
            var grid = this.SeatsGrid;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // À l'intérieur de votre boucle foreach où vous créez les ToggleButtons
            foreach (var ticket in viewModel.Tickets)
            {
                var button = new ToggleButton
                {
                    Margin = new Thickness(2),
                    IsChecked = false,
                    IsEnabled = false,
                    Tag = ticket,
                    Foreground = GetColorFromSectionName(ticket.Seat.SectionName),
                    Style = (Style)FindResource("SeatToggleButtonStyle"),
                };

                // Ajouter les gestionnaires d'événements
                button.Checked += async (sender, e) => await ToggleSeatStatusAsync(sender, true);
                button.Unchecked += async (sender, e) => await ToggleSeatStatusAsync(sender, false);

                // Positionnez le bouton sur le Grid
                if (ticket.Seat.XCoordinate >= 0 && ticket.Seat.YCoordinate >= 0)
                {
                    Grid.SetRow(button, ticket.Seat.YCoordinate);
                    Grid.SetColumn(button, ticket.Seat.XCoordinate);
                    grid.Children.Add(button);
                }
            }

            viewModel._isSeatLoad = true;
            viewModel.SelectedFilter = viewModel.Filters[0];
        }
        private async Task ToggleSeatStatusAsync(object sender, bool isChecked)
        {
            if (sender is ToggleButton button && button.Tag is Ticket ticket)
            {
                try
                {
                    if (isChecked)
                    {
                        // Le siège est sélectionné, réservez le billet
                        await DAL.TicketFactory.ReserveAsync(ticket);
                    }
                    else
                    {
                        // Le siège est désélectionné, rendez le billet disponible
                        await DAL.TicketFactory.MakeAvailableAsync(ticket);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }





        public async Task SelectBestAdjacentSeats(int numberOfSeatsToSelect, string filter)
        {
            var viewModel = DataContext as VMSeatSelection;
            if (viewModel == null || viewModel.Auditorium == null) return;

            foreach (var prevSelectedSeat in viewModel.SelectedTickets)
            {
                ToggleSeat(prevSelectedSeat, false);
            }
            viewModel.SelectedTickets.Clear();

            IEnumerable<Ticket> filteredTicket;

            // Filtrez les sièges en fonction du filtre sélectionné
            if (filter == "Section")
            {
                // Convertir le nom de la section sélectionnée en enum
                if (Enum.TryParse<SectionName>(viewModel.SelectedSectionName, out var selectedSection))
                {
                    filteredTicket = viewModel.Tickets
                        .Where(ticket => ticket.Seat.Status == SeatStatus.InService && ticket.Seat.SectionName == selectedSection);
                }
                else
                {
                    // Si la conversion échoue, aucun siège n'est sélectionné
                    return;
                }
            }
            else // Filtre "Salle", sélectionnez tous les sièges disponibles
            {
                filteredTicket = viewModel.Tickets
                    .Where(ticket => ticket.Seat.Status == SeatStatus.InService
                     && ticket.TicketStatus == TicketStatus.Available
                     && ticket.Seat.SectionName != SectionName.Loge);
            }

            // Trier les sièges par rangée et numéro de siège
            var orderedSeats = filteredTicket
                .OrderBy(ticket => ticket.Seat.YCoordinate)
                .ThenBy(ticket => ticket.Seat.SeatNumber % 2 == 0 ? 1 : 0)
                .ThenBy(ticket => ticket.Seat.SeatNumber)
                .ToList();


            List<Ticket> selectedSeats = new List<Ticket>();

            foreach (var rowGroup in orderedSeats.GroupBy(ticket => ticket.Seat.RowName))
            {
                var seatsInRow = rowGroup.ToList();

                for (int i = 0; i <= seatsInRow.Count - numberOfSeatsToSelect; i++)
                {
                    var potentialGroup = seatsInRow.GetRange(i, numberOfSeatsToSelect);
                    bool allSeatsAvailable = true;

                    foreach (var ticket in potentialGroup)
                    {
                        if (!(ticket.TicketStatus == TicketStatus.Available))
                        {
                            allSeatsAvailable = false;
                            break;
                        }
                    }

                    if (allSeatsAvailable)
                    {
                        selectedSeats.AddRange(potentialGroup);
                        break;
                    }
                }

                if (selectedSeats.Count == numberOfSeatsToSelect) break;
            }

            foreach (var seat in selectedSeats)
            {
                seat.Representation = viewModel.Representation;
                ToggleSeat(seat, true);
            }
            viewModel.SelectedTickets.AddRange(selectedSeats);
        }

        private void ToggleSeat(Ticket ticket, bool isSelected)
        {
            // Trouver le ToggleButton correspondant au siège
            var button = SeatsGrid.Children
                .OfType<ToggleButton>()
                .FirstOrDefault(tb => tb.Tag is Ticket && ((Ticket)tb.Tag).Seat.Id == ticket.Seat.Id);

            if (button != null)
            {
                button.IsEnabled = isSelected;
                button.IsChecked = isSelected;
            }
        }


        private Brush GetColorFromSectionName(SectionName sectionName)
        {
            switch (sectionName)
            {
                case SectionName.Parterre: return Brushes.Green;
                case SectionName.Balcon: return Brushes.Purple;
                case SectionName.Loge: return Brushes.Blue;
                default: return Brushes.Gray;
            }
        }
    }
}
