using AppGestion.DataAccessLayer;
using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mysqlx.Cursor;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMManageEvent : ObservableObject
    {
        #region properties

        /// <summary>
        /// The list of active show
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Show> _Shows;

        /// <summary>
        /// The selected show
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenEditEventCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteShowCommand))]
        private Show _selectedShow;

        /// <summary>
        /// The list of active representation for the selected show
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedShow))]
        private ObservableCollection<Representation> _representations;

        /// <summary>
        /// The selected representation
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteRepresentationCommand))]
        private Representation _selectedRepresentation;

        private bool _isInitialLoadComplete = false;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMManageEvent()
        {
            InitializeProperties();
            LoadData();
        }

        #endregion


        #region commands

        /// <summary>
        /// Command to open the AddEvent window
        /// </summary>
        [RelayCommand]
        public void OpenAddEvent()
        {
            // Open the AddEditEvent window
            AddEditEvent Addwindow = new AddEditEvent();
            Addwindow.ShowDialog();

            // Reload the data
            LoadData();
        }

        /// <summary>
        /// Command to open the EditEvent window
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanEditShow))]
        public void OpenEditEvent()
        {
            // Open the AddEditEvent window
            AddEditEvent Editwindow = new AddEditEvent(SelectedShow);
            Editwindow.ShowDialog();

            // Reload the data
            LoadData();
        }

        /// <summary>
        /// Command to delete the selected show
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanEditShow))]
        public async void DeleteShow()
        {
            try
            {
                // Delete the selected show
                await DAL.ShowFactory.SetInactiveAsync(SelectedShow.Id);

                // Remove the show from the list
                Shows.Remove(SelectedShow);
            }
            catch (Exception e)
            {
                // Pop a message box
                System.Windows.MessageBox.Show(e.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Check if the selected show is not null
        /// </summary>
        /// <returns></returns>
        public bool CanEditShow()
        {
            return SelectedShow is not null;
        }

        /// <summary>
        /// Command to open the addRepresantation window
        /// </summary>
        [RelayCommand]
        public void OpenAddRepresantation()
        {
            // Open the AddEditEvent window
            AddEditRepresentation Addwindow = new AddEditRepresentation(SelectedShow);
            Addwindow.ShowDialog();

            LoadData();
        }

        /// <summary>
        /// Command to delete the selected representation
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanEditRepresentation))]
        public async void DeleteRepresentation()
        {
            try
            {
                // Delete the selected representation
                await DAL.RepresentationFactory.SetAsInactiveAsync(SelectedRepresentation);

                // Remove the representation from the list
                Representations.Remove(SelectedRepresentation);
            }
            catch (Exception e)
            {
                // Pop a message box
                System.Windows.MessageBox.Show(e.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Check if the selected representation is not null
        /// </summary>
        /// <returns></returns>
        public bool CanEditRepresentation()
        {
            return SelectedRepresentation is not null;
        }


        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            Shows = new ObservableCollection<Show>();
            SelectedShow = new Show();
            Representations = new ObservableCollection<Representation>();
            SelectedRepresentation = new Representation();
        }

        /// <summary>
        /// Load the data
        /// </summary>
        private async Task LoadData()
        {
            // Clear the list of show
            Shows.Clear();

            // Get the list of show
            var shows = await DAL.ShowFactory.GetAllActiveAsync();

            // Add the show to the list
            foreach (var show in shows)
            {
                Shows.Add(show);
            }

            // if the list is not empty
            if (Shows.Count > 0)
            {
                // Set the selected show
                SelectedShow = Shows[0];
            }

            LoadRepresentation();

            _isInitialLoadComplete = true;

        }

        /// <summary>
        /// Load the representation for the selected show
        /// </summary>
        private async void LoadRepresentation()
        {
            // Clear the list of representation
            Representations.Clear();

            SelectedRepresentation = null;

            if(SelectedShow != null)
            {
                // Get the list of representation
                var representations = await DAL.RepresentationFactory.GetByShowAsync(SelectedShow);

                // Add the representation to the list
                foreach (var representation in representations)
                {
                    Representations.Add(representation);
                }

                // if the list is not empty
                if (Representations.Count > 0)
                {
                    // Set the selected representation
                    SelectedRepresentation = Representations[0];
                }
            }
        }

        /// <summary>
        /// Method called when the selected show changes
        /// </summary>
        partial void OnSelectedShowChanged(Show value)
        {
            if (Representations is not null && _isInitialLoadComplete)
            {
                LoadRepresentation();
            }
        }


        #endregion
    }
}
