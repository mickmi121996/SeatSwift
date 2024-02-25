using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAddEditEvent : ObservableObject
    {
        #region properties

        /// <summary>
        /// The title of the window
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// The selected image path
        /// </summary>
        [ObservableProperty]
        private string _selectedImagePath;

        /// <summary>
        /// The event name
        /// </summary>
        [ObservableProperty]
        private string _eventName;

        /// <summary>
        /// The event description
        /// </summary>
        [ObservableProperty]
        private string _eventDescription;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMAddEditEvent()
        {
            _title = string.Empty;
            _selectedImagePath = string.Empty;
            _eventName = string.Empty;
            _eventDescription = string.Empty;
        }

        #endregion


        #region commands

        [RelayCommand]
        private async Task BrowseImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*",
                Title = "Select an event image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Assumer que vous avez une propriété pour stocker le chemin de l'image
                SelectedImagePath = openFileDialog.FileName;
            }

            await Task.CompletedTask;
        }

        #endregion


        #region methods

        /// <summary>
        /// Initializes the properties of the VMAddEditEvent
        /// </summary>
        public void Initialize()
        {
            Title = "Add Event";
            EventName = string.Empty;
            EventDescription = string.Empty;
            SelectedImagePath = string.Empty;
        }

        #endregion
    }
}
