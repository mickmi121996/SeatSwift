using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMManageTheater : ObservableObject
    {
        #region properties

        /// <summary>
        /// A list of Auditoriums
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Auditorium> _auditoriums;

        /// <summary>
        /// A list of Auditoriums
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Seat> _seats;

        /// <summary>
        /// The selected auditorium
        /// </summary>
        [ObservableProperty]
        private Auditorium _selectedAuditorium;

        public event Action SeatsInitialized;

        private bool _isInitialLoadComplete = false;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMManageTheater()
        {
            InitializeProperties();
            GetAuditoriums();
        }

        #endregion


        #region commands



        #endregion


        #region methods

        [RelayCommand]
        public async Task ToggleSeat(Seat seat)
        {
            if (seat != null)
            {
                seat.Status = seat.Status == SeatStatus.InService ? SeatStatus.OutOfService : SeatStatus.InService;
                // Mettre à jour la base de données ou le service web si nécessaire ici
                try
                {
                    await DAL.SeatFactory.UpdateStatusAsync(seat.Id, seat.Status);
                }
                catch (Exception ex)
                {
                    // Message box error
                    MessageBox.Show("Erreur lors de la mise à jour du siège.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            _isInitialLoadComplete = false;
            Auditoriums = new ObservableCollection<Auditorium>();
            Seats = new ObservableCollection<Seat>();
            SelectedAuditorium = new Auditorium();
        }


        /// <summary>
        /// Method called when the selected Auditorium changes
        /// </summary>
        partial void OnSelectedAuditoriumChanged(Auditorium value)
        {
            if (value is not null && value.Id>0)
            {
                _ = InitializeSeats();
            }
        }

        /// <summary>
        /// Method to get the list of auditoriums
        /// </summary>
        private async void GetAuditoriums()
        {
            var auditoriums = await DAL.AuditoriumFactory.GetAllActiveAsync();
            Auditoriums.Insert(0, new Auditorium { Name = "Sélectionner un auditorium" });

            // Create the list of auditoriums
            foreach (var auditorium in auditoriums)
            {
                Auditoriums.Add(auditorium);
            }

            if (auditoriums.Count > 0)
            {
                SelectedAuditorium = Auditoriums.FirstOrDefault();
            }
        }

        public async Task InitializeSeats()
        {
            VMMainWindow.Instance.IsCurrentlyWorking = Visibility.Visible;
            var seatList = await DAL.SeatFactory.GetAllByAuditoriumIdSimpleAsync(SelectedAuditorium.Id);
            foreach (var seat in seatList)
            {
                seat.XCoordinate -= 1;
                Seats.Add(seat);
            }
            SeatsInitialized?.Invoke();
        }

        #endregion
    }
}
