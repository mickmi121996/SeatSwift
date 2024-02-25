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
    public partial class VMManageEmploye : ObservableObject
    {
        #region properties



        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMManageEmploye()
        {
            InitializeProperties();
        }

        #endregion


        #region commands

        /// <summary>
        /// Command to open the AddEmploye window
        /// </summary>
        [RelayCommand]
        public void OpenAddEmploye()
        {
            // Open the AddEditEmploye window
            AddEditEmploye Addwindow = new AddEditEmploye();
            Addwindow.ShowDialog();
        }

        /// <summary>
        /// Command to open the EditEmploye window
        /// </summary>
        [RelayCommand]
        public void OpenEditEmploye()
        {
            // Open the AddEditEmploye window
            AddEditEmploye Editwindow = new AddEditEmploye();
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
