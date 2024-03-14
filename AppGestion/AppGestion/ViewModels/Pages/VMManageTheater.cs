using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        /// The selected auditorium
        /// </summary>
        [ObservableProperty]
        private Auditorium _selectedAuditorium;

        /// <summary>
        /// The selected auditorium
        /// </summary>
        [ObservableProperty]
        private AuditoriumViewModel _auditoriumViewModel;


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

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            Auditoriums = new ObservableCollection<Auditorium>();
            SelectedAuditorium = new Auditorium();
        }

        /// <summary>
        /// Method called when the selected Auditorium changes
        /// </summary>
        partial void OnSelectedAuditoriumChanged(Auditorium value)
        {
            if (value is not null)
            {
                // Initialisez le tableau 2D avec les dimensions de l'auditorium sélectionné.
                AuditoriumViewModel = new AuditoriumViewModel(value);
            }
        }


        /// <summary>
        /// Method to get the list of auditoriums
        /// </summary>
        private async void GetAuditoriums()
        {
            var auditoriums = await DAL.AuditoriumFactory.GetAllActiveAsync();

            // Create the list of auditoriums
            foreach (var auditorium in auditoriums)
            {
                Auditoriums.Add(auditorium);
            }

            // Set the selected auditorium
            if (Auditoriums.Count > 0)
            {
                SelectedAuditorium = Auditoriums[0];
            }
        }

        #endregion
    }
}
