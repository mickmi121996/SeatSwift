using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels
{
    // Définissez une classe pour le siège qui inclut les propriétés nécessaires pour le binding
    public partial class SeatViewModel : ObservableObject
    {
        /// <summary>
        /// The seat number
        /// </summary>
        public int SeatNumber { get; set; }

        /// <summary>
        /// The row name
        /// </summary>
        public string RowName { get; set; }

        /// <summary>
        /// The seat status
        /// </summary>
        [ObservableProperty]
        private bool _isInService;

        /// <summary>
        /// The section name
        /// </summary>
        public string SectionName { get; set; }
        
        /// <summary>
        /// The seat 
        /// </summary>
        public Seat Seat { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SeatViewModel(Seat seat)
        {
            Seat = seat;
            SeatNumber = seat.SeatNumber;
            RowName = seat.RowName;
            SectionName = seat.SectionName.ToString();
            IsInService = seat.Status == SeatStatus.InService;
        }

        /// <summary>
        /// Command to toggle the seat status
        /// </summary>
        [RelayCommand]
        public async Task ToggleSeatStatus()
        {
            IsInService = !IsInService;

            Seat.Status = IsInService ? SeatStatus.InService : SeatStatus.OutOfService;

            // Envoie la mise à jour à la base de données
            await DAL.SeatFactory.UpdateStatusAsync(Seat.Id, Seat.Status);
        }

        /// <summary>
        /// Change the seat status when the seat is clicked
        /// </summary>
        public void ChangeSeatStatus()
        {
            Seat.Status = IsInService ? SeatStatus.InService : SeatStatus.OutOfService;
        }
    }

}
