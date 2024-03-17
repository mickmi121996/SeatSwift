using AppGestion.DataAccessLayer;
using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAddEditRepresentation : ObservableObject
    {
        #region properties

        /// <summary>
        /// The current date
        /// </summary>
        [ObservableProperty]
        private DateTime _currentDate;

        /// <summary>
        /// The current time
        /// </summary>
        [ObservableProperty]
        private DateTime _currentTime;

        /// <summary>
        /// The title of the window
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// The selected date
        /// </summary>
        [ObservableProperty]
        private DateTime _selectedDate;

        /// <summary>
        /// The is creating ticket
        /// </summary>
        [ObservableProperty]
        private bool isCreatingTickets;

        /// <summary>
        /// The is creating ticket
        /// </summary>
        [ObservableProperty]
        private Visibility isCreatingTicketsVisibility;

        /// <summary>
        /// The selected time
        /// </summary>
        [ObservableProperty]
        private DateTime _selectedTime;

        /// <summary>
        /// The representation
        /// </summary>
        [ObservableProperty]
        private Representation _representation;

        /// <summary>
        /// The list of auditorium
        /// </summary>
        [ObservableProperty]
        private List<Auditorium> _auditoriums;

        /// <summary>
        /// The selected auditorium
        /// </summary>
        [ObservableProperty]
        private Auditorium _selectedAuditorium;

        /// <summary>
        /// The show
        /// </summary>
        [ObservableProperty]
        private Show _show;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedDate))]
        [NotifyPropertyChangedFor(nameof(SelectedTime))]
        private DateTime _selectedDateTime;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMAddEditRepresentation(Show show)
        {
            if (show is not null)
            {
                _show = show;
                InitializeProperties();
            }

            GetAuditoriums();
        }

        #endregion


        #region commands

        /// <summary>
        /// Command to confirm the representation
        /// </summary>
        [RelayCommand]
        public async Task Confirm()
        {
            try
            {
                IsCreatingTicketsVisibility = Visibility.Visible;
                // Create the representation
                Representation representation = new Representation()
                {
                    Show = Show,
                    Auditorium = SelectedAuditorium,
                    Date = SelectedDateTime,
                    Status = RepresentationStatus.Available
                };

                // Add the representation to the database
                await DAL.RepresentationFactory.CreateAsync(representation);

                // Get a representation in the database
                Representation = await DAL.RepresentationFactory.GetByShowAndDateAsync(
                    Show,
                    SelectedDateTime
                );

                // Create the tickets
                await CreateTickets();

                IsCreatingTicketsVisibility = Visibility.Hidden;

                // Close the window
                Application.Current.Windows
                    .OfType<AddEditRepresentation>()
                    .FirstOrDefault()
                    ?.Close();
            }
            catch (Exception ex)
            {
                IsCreatingTicketsVisibility = Visibility.Hidden;
                // Open a message box
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            GetCurrentDate();
            IsCreatingTicketsVisibility = Visibility.Hidden;
            Title = "Nouvelle représentation pour " + Show.Name;
            SelectedDate = CurrentDate;
            SelectedTime = CurrentTime;
            Auditoriums = new List<Auditorium>();
        }

        /// <summary>
        /// Get the list of auditorium
        /// </summary>
        private async void GetAuditoriums()
        {
            if (Auditoriums is not null)
            {
                // Clear the list
                Auditoriums.Clear();

                // Get the list of auditorium
                var auditoriums = await DAL.AuditoriumFactory.GetAllActiveAsync();

                // Add the auditorium to the list
                foreach (var auditorium in auditoriums)
                {
                    Auditoriums.Add(auditorium);
                }

                // if the list is not empty
                if (Auditoriums.Count > 0)
                {
                    // Set the selected auditorium
                    SelectedAuditorium = Auditoriums[0];
                }
            }
        }

        /// <summary>
        /// Method to create all the tickets for the representation
        /// </summary>
        private async Task CreateTickets()
        {
            if (Representation.Auditorium is not null)
            {
                // Get the list of seats
                var seats = await DAL.SeatFactory.GetAllByAuditoriumIdAsync(
                    Representation.Auditorium.Id
                );

                // Create a list of tickets
                List<Ticket> tickets = new List<Ticket>();

                // Create a ticket for each seat
                foreach (var seat in seats)
                {
                    Ticket ticket = new Ticket()
                    {
                        IsActive = true,
                        Representation = Representation,
                        Seat = seat,
                        ReservationNumber = $"{Representation.Id}{seat.Id}",
                        TicketStatus = TicketStatus.Available
                    };

                    tickets.Add(ticket);
                }

                // Add the tickets to the database
                await DAL.TicketFactory.CreateRangeAsync(tickets);
            }
        }

        /// <summary>
        /// Method called when the selected show changes
        /// </summary>
        partial void OnSelectedDateChanged(DateTime value)
        {
            UpdateSelectedDateTime();
        }

        /// <summary>
        /// Method called when the selected show changes
        /// </summary>
        partial void OnSelectedTimeChanged(DateTime value)
        {
            UpdateSelectedDateTime();
        }

        private void UpdateSelectedDateTime()
        {
            if (SelectedDate != null && SelectedTime != null)
            {
                SelectedDateTime = new DateTime(
                    SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day,
                    SelectedTime.Hour,
                    SelectedTime.Minute,
                    SelectedTime.Second
                );
            }
        }

        /// <summary>
        /// Get the current date
        /// </summary>
        private void GetCurrentDate()
        {
            CurrentDate = DateTime.Now;
        }

        #endregion
    }
}
