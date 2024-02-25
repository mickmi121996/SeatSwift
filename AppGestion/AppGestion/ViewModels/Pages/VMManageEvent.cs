using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMManageEvent : ObservableObject
    {
        #region properties



        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMManageEvent()
        {
            InitializeProperties();
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
        }

        /// <summary>
        /// Command to open the EditEvent window
        /// </summary>
        [RelayCommand]
        public void OpenEditEvent()
        {
            // Open the AddEditEvent window
            AddEditEvent Editwindow = new AddEditEvent();
            Editwindow.ShowDialog();
        }

        /// <summary>
        /// Command to open the addRepresantation window
        /// </summary>
        [RelayCommand]
        public void OpenAddRepresantation()
        {
            // Open the AddEditEvent window
            AddEditRepresentation Addwindow = new AddEditRepresentation();
            Addwindow.ShowDialog();
        }

        /// <summary>
        /// Command to open the EditRepresantation window
        /// </summary>
        [RelayCommand]
        public void OpenEditRepresantation()
        {
            // Open the AddEditEvent window
            AddEditRepresentation Editwindow = new AddEditRepresentation();
            Editwindow.ShowDialog();
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            // Initialize the properties
        }

        #endregion
    }
}
