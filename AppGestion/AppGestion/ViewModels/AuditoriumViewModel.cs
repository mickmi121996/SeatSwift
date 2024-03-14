using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using System.Collections.ObjectModel;

namespace AppGestion.ViewModels
{
    public partial class AuditoriumViewModel : ObservableObject
    {
        [ObservableProperty]
        private SeatViewModel[,] _seatsGrid;

        /// <summary>
        /// The auditorium
        /// </summary>
        [ObservableProperty]
        private Auditorium _auditorium;

        public AuditoriumViewModel(Auditorium auditorium)
        {
            Auditorium = auditorium;
        }

        private async void InitializeSeats()
        {
            // Simulons l'initialisation du tableau 2D basée sur les dimensions de l'auditorium
            _seatsGrid = new SeatViewModel[Auditorium.NumberOfRows, Auditorium.NumberOfColumns];

            var seatList = await DAL.SeatFactory.GetAllByAuditoriumIdAsync(Auditorium.Id);
            foreach (var seat in seatList)
            {
                var seatVm = new SeatViewModel(seat);
                _seatsGrid[seat.YCoordinate - 1, seat.XCoordinate - 1] = seatVm;
            }
        }

    }
}
