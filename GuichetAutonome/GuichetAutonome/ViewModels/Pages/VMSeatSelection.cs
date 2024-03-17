using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.Views.Pages;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMSeatSelection : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The visibility of the Selection filter
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedFilter))]
        private Visibility _isSelectionFilterVisible;

        /// <summary>
        /// The observable collection of SectionName
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _sectionNames;

        /// <summary>
        /// The selected section name
        /// </summary>
        [ObservableProperty]
        private string _selectedSectionName;

        /// <summary>
        /// The observable collection of Filter
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _filters;

        /// <summary>
        /// The selected filter
        /// </summary>
        [ObservableProperty]
        private string _selectedFilter;

        /// <summary>
        /// The observable collection of tickets for the representation
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Ticket> _tickets;

        /// <summary>
        /// The observable collection of the selected ticket
        /// </summary>
        [ObservableProperty]
        private List<Ticket> _selectedTickets;

        /// <summary>
        /// The observable collection of tickets for the representation
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Seat> _seats;

        /// <summary>
        /// The representation
        /// </summary>
        [ObservableProperty]
        private Representation _representation;

        /// <summary>
        /// The representation
        /// </summary>
        [ObservableProperty]
        private Auditorium _auditorium;

        /// <summary>
        /// The number of tickets
        /// </summary>
        [ObservableProperty]
        private int _numberOfTickets;

        public bool _isSeatLoad;

        #endregion


        #region Constructor

        public VMSeatSelection(Representation representation, int numberOfTicket)
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            _seats = new ObservableCollection<Seat>();
            _isSelectionFilterVisible = Visibility.Collapsed;
            _selectedTickets = new List<Ticket>();
            _isSeatLoad = false;

            _representation = representation;
            _numberOfTickets = numberOfTicket;

            InitializeSeatsAndTickets();

            InitializeFilterAndSectionName();
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the event selection page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToEventSelection()
        {
            if (_selectedTickets != null)
            {
                foreach (var ticket in _selectedTickets)
                {
                    await DAL.TicketFactory.MakeAvailableAsync(ticket);
                }

                _selectedTickets.Clear();
            }

            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        /// <summary>
        /// Command to add the order to the cart
        /// </summary>
        [RelayCommand]
        public void AddOrderToCart()
        {
            VMMainWindow.Instance.Cart.AddRange(SelectedTickets);
            // Add the order to the cart
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize The list of filter and section name
        /// </summary>
        private void InitializeFilterAndSectionName()
        {
            _sectionNames = new ObservableCollection<string>();
            _filters = new ObservableCollection<string>();
            _tickets = new ObservableCollection<Ticket>();

            // Get the list of section name in the enumeration
            foreach (var section in Enum.GetValues(typeof(SectionName)))
            {
                SectionNames.Add(section.ToString());
            }

            // Get the list of filter with Salle and Section
            Filters.Add("Salle");
            Filters.Add("Section");
        }

        [RelayCommand]
        public async Task ToggleSeat(Seat seat)
        {
            if (seat != null)
            {
                seat.Status =
                    seat.Status == SeatStatus.InService
                        ? SeatStatus.OutOfService
                        : SeatStatus.InService;
                try
                {
                    await DAL.SeatFactory.UpdateStatusAsync(seat.Id, seat.Status);
                }
                catch (Exception ex)
                {
                    // Message box error
                    MessageBox.Show(
                        "Erreur lors de la mise à jour du siège.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        public event Action SeatsInitialized;

        /// <summary>
        /// Get the list of seat for the selected representation
        /// </summary>
        public async Task InitializeSeatsAndTickets()
        {
            if (Representation.Auditorium is not null)
            {
                Auditorium = await DAL.AuditoriumFactory.GetByIdAsync(Representation.Auditorium.Id);

                var seatList = await DAL.SeatFactory.GetAllByAuditoriumIdAsync(Auditorium.Id);

                var tickets = await DAL.TicketFactory.GetByRepresentationAsync(Representation);

                foreach (var seat in seatList)
                {
                    seat.XCoordinate -= 1;
                    Seats.Add(seat);
                }

                foreach (var ticket in tickets)
                {
                    var matchingSeat = seatList.FirstOrDefault(seat => seat.Id == ticket.SeatId);
                    if (matchingSeat != null)
                    {
                        ticket.Seat = matchingSeat;
                        Tickets.Add(ticket);
                    }
                }

                SeatsInitialized?.Invoke();
            }
        }

        public event Action FilterOrSectionChanged;

        /// <summary>
        /// Method called when the selected Auditorium changes
        /// </summary>
        partial void OnSelectedFilterChanged(string value)
        {
            if (value != null && _isSeatLoad)
            {
                // Check if the selected filter is Salle
                if (value == "Salle")
                {
                    IsSelectionFilterVisible = Visibility.Collapsed;
                }
                else
                {
                    IsSelectionFilterVisible = Visibility.Visible;
                    SelectedSectionName = SectionNames.FirstOrDefault();
                }

                FilterOrSectionChanged?.Invoke();
            }
        }

        partial void OnSelectedSectionNameChanged(string value)
        {
            FilterOrSectionChanged?.Invoke();
        }

        #endregion
    }
}
