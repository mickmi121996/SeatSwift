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
            foreach (var seat in viewModel.Seats)
            {
                Ticket ticket = await DAL.TicketFactory.GetByRepresentationAndSeatAsync(viewModel.Representation, seat);

                var button = new ToggleButton
                {
                    Margin = new Thickness(2),
                    IsChecked = false,
                    IsEnabled = ticket.TicketStatus == TicketStatus.Available && seat.Status == SeatStatus.InService,
                    Style = (Style)FindResource("SeatToggleButtonStyle"),
                };

                // Positionnez le bouton sur le Grid
                if (seat.XCoordinate >= 0 && seat.YCoordinate >= 0)
                {
                    Grid.SetRow(button, seat.YCoordinate);
                    Grid.SetColumn(button, seat.XCoordinate);
                    grid.Children.Add(button);
                }
            }
        }



        public async Task SelectBestAdjacentSeats(int numberOfSeatsToSelect, string filter)
        {
            var viewModel = DataContext as VMSeatSelection;
            if (viewModel == null || viewModel.Auditorium == null) return;

            IEnumerable<Seat> filteredSeats;

            // Filtrez les sièges en fonction du filtre sélectionné
            if (filter == "Section")
            {
                // Convertir le nom de la section sélectionnée en enum
                if (Enum.TryParse<SectionName>(viewModel.SelectedSectionName, out var selectedSection))
                {
                    filteredSeats = viewModel.Seats
                        .Where(seat => seat.Status == SeatStatus.InService && seat.SectionName == selectedSection);
                }
                else
                {
                    // Si la conversion échoue, aucun siège n'est sélectionné
                    return;
                }
            }
            else // Filtre "Salle", sélectionnez tous les sièges disponibles
            {
                filteredSeats = viewModel.Seats
                    .Where(seat => seat.Status == SeatStatus.InService);
            }

            // Trier les sièges par rangée et numéro de siège
            var orderedSeats = filteredSeats
                .OrderBy(seat => seat.RowName) // Trier par rangée pour "devant"
                .ThenBy(seat => seat.XCoordinate) // Trier par coordonnée X pour l'ordre de siège
                .ToList();

            List<Seat> selectedSeats = new List<Seat>();

            foreach (var rowGroup in orderedSeats.GroupBy(seat => seat.RowName))
            {
                var seatsInRow = rowGroup.ToList();

                for (int i = 0; i <= seatsInRow.Count - numberOfSeatsToSelect; i++)
                {
                    var potentialGroup = seatsInRow.GetRange(i, numberOfSeatsToSelect);
                    bool allSeatsAvailable = true;

                    foreach (var seat in potentialGroup)
                    {
                        if (!await CheckSeatAvailability(seat))
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
                //await ToggleSeat(seat);
            }
        }


        // Méthode pour vérifier la disponibilité des sièges
        private async Task<bool> CheckSeatAvailability(Seat seat)
        {
            var viewModel = DataContext as VMSeatSelection;
            if (viewModel == null) throw new InvalidOperationException("DataContext must be set to an instance of VMSeatSelection.");

            var ticket = await DAL.TicketFactory.GetByRepresentationAndSeatAsync(viewModel.Representation, seat);
            return ticket.TicketStatus == TicketStatus.Available;
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
