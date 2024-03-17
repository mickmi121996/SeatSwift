using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.ViewModels.Pages;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

            vMSeatSelection.SeatsInitialized += ViewModel_SeatsInitialized;

            vMSeatSelection.FilterOrSectionChanged += async () =>
            {
                await SelectBestAdjacentSeats(
                    vMSeatSelection.NumberOfTickets,
                    vMSeatSelection.SelectedFilter
                );
            };

            this.Unloaded += ManageTheater_Unloaded;
        }

        private void ViewModel_SeatsInitialized()
        {
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
            if (viewModel == null || viewModel.Auditorium == null)
                return;

            var rows = viewModel.Auditorium.NumberOfRows;
            var columns = viewModel.Auditorium.NumberOfColumns;

            var grid = this.SeatsGrid;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                );
            }

            for (int j = 0; j < columns; j++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                );
            }

            foreach (var ticket in viewModel.Tickets)
            {
                var button = new ToggleButton
                {
                    Margin = new Thickness(2),
                    IsChecked = false,
                    IsEnabled = false,
                    Tag = ticket,
                    Style = (Style)FindResource("SeatToggleButtonStyle"),
                };

                button.Checked += async (sender, e) => await ToggleSeatStatusAsync(sender, true);
                button.Unchecked += async (sender, e) => await ToggleSeatStatusAsync(sender, false);

                if (ticket.Seat.XCoordinate >= 0 && ticket.Seat.YCoordinate >= 0)
                {
                    if(ticket.Seat.Status == SeatStatus.OutOfService)
                    {
                        button.Background = new SolidColorBrush(Colors.DarkRed);
                    }

                    if (ticket.TicketStatus == TicketStatus.Reserved || ticket.TicketStatus == TicketStatus.Purchased)
                    {
                        button.Background = new SolidColorBrush(Colors.DarkOrange);
                    }

                    if(button.IsChecked == true) 
                    {
                        button.Background = new SolidColorBrush(Colors.DarkGreen);
                    }

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
                        await DAL.TicketFactory.ReserveAsync(ticket);
                    }
                    else
                    {
                        await DAL.TicketFactory.MakeAvailableAsync(ticket);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        public async Task SelectBestAdjacentSeats(int numberOfSeatsToSelect, string filter)
        {
            var viewModel = DataContext as VMSeatSelection;
            if (viewModel == null || viewModel.Auditorium == null)
                return;

            foreach (var prevSelectedSeat in viewModel.SelectedTickets)
            {
                ToggleSeat(prevSelectedSeat, false);
            }
            viewModel.SelectedTickets.Clear();

            IEnumerable<Ticket> filteredTicket;

            if (filter == "Section")
            {
                if (
                    Enum.TryParse<SectionName>(
                        viewModel.SelectedSectionName,
                        out var selectedSection
                    )
                )
                {
                    filteredTicket = viewModel.Tickets.Where(
                        ticket =>
                            ticket.Seat.Status == SeatStatus.InService
                            && ticket.Seat.SectionName == selectedSection
                    );
                }
                else
                {
                    return;
                }
            }
            else
            {
                filteredTicket = viewModel.Tickets.Where(
                    ticket =>
                        ticket.Seat.Status == SeatStatus.InService
                        && ticket.TicketStatus == TicketStatus.Available
                        && ticket.Seat.SectionName != SectionName.Loge
                );
            }

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

                if (selectedSeats.Count == numberOfSeatsToSelect)
                    break;
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
            var button = SeatsGrid.Children
                .OfType<ToggleButton>()
                .FirstOrDefault(
                    tb => tb.Tag is Ticket && ((Ticket)tb.Tag).Seat.Id == ticket.Seat.Id
                );

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
                case SectionName.Parterre:
                    return Brushes.Green;
                case SectionName.Balcon:
                    return Brushes.Purple;
                case SectionName.Loge:
                    return Brushes.Blue;
                default:
                    return Brushes.Gray;
            }
        }
    }
}
