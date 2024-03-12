using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.UserControls
{
    public partial class VMEventTemplate : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The show
        /// </summary>
        [ObservableProperty]
        private Show _show;

        /// <summary>
        /// The list of Auditoriums
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Auditorium> _auditoriums;

        /// <summary>
        /// The selected auditorium
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
        private Auditorium _selectedAuditorium;

        /// <summary>
        /// THe list of representations
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedAuditorium))]
        private ObservableCollection<Representation> _representations;

        /// <summary>
        /// The selected representation
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
        private Representation _selectedRepresentation;

        /// <summary>
        /// The number of tickets
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ContinueCommand))]
        private int _numberOfTickets;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMEventTemplate(Show show)
        {
            Show = show;
            InitializeProperties();
        }

        #endregion


        #region Command

        /// <summary>
        /// Command to continue
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanContinue))]
        public void Continue()
        {

        }

        /// <summary>
        /// The can execute for the continue command
        /// </summary>
        /// <returns></returns>
        private bool CanContinue()
        {
            if (
                SelectedAuditorium is null ||
                SelectedRepresentation is null ||
                NumberOfTickets <= 0
                )
            {
                return false;
            }
            return true;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            Auditoriums = new ObservableCollection<Auditorium>();
            Representations = new ObservableCollection<Representation>();
            NumberOfTickets = 1;
            LoadAuditoriums();
        }

        /// <summary>
        /// Load the auditoriums
        /// </summary>
        private async Task LoadAuditoriums()
        {
            // clear the list
            Auditoriums.Clear();

            // get the auditoriums
            var auditoriums = await DAL.AuditoriumFactory.GetByShowAsync(Show);

            // add the auditoriums to the list
            foreach (var auditorium in auditoriums)
            {
                Auditoriums.Add(auditorium);
            }

            // if there is at least one auditorium
            if (Auditoriums.Count > 0)
            {
                // select the first auditorium
                SelectedAuditorium = Auditoriums[0];
            }
        }

        /// <summary>
        /// Load the representations
        /// </summary>
        private async Task LoadRepresentations()
        {
            // clear the list
            Representations.Clear();

            // get the representations
            var representations = await DAL.RepresentationFactory.GetInComingActiveAvailableByShowAndAuditoriumAsync(Show, SelectedAuditorium);

            // add the representations to the list
            foreach (var representation in representations)
            {
                Representations.Add(representation);
            }

            // if there is at least one representation
            if (Representations.Count > 0)
            {
                // select the first representation
                SelectedRepresentation = Representations[0];
            }
        }

        /// <summary>
        /// Method called when the selected Auditorium changes
        /// </summary>
        partial void OnSelectedAuditoriumChanged(Auditorium value)
        {
            if (value != null)
            {
                LoadRepresentations();
            }
        }

        #endregion
    }
}
