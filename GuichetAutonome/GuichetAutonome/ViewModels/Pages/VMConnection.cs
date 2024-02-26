using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views;
using GuichetAutonome.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMConnection : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        [ObservableProperty]
        private string _email;

        /// <summary>
        /// The password of the user
        /// </summary>
        [ObservableProperty]
        private string _password;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMConnection()
        {
            InitializeProperties();
        }

        #endregion


        #region Commands

        /// <summary>
        /// The command to connect the user
        /// </summary>
        [RelayCommand]
        public async Task Connect()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        /// <summary>
        /// The command to go to the registration page
        /// </summary>
        [RelayCommand]
        public async Task GoToRegistration()
        {
            VMMainWindow.Instance.ChangePage(typeof(Registration));
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initializes the properties
        /// </summary>
        private void InitializeProperties()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        #endregion
    }
}
