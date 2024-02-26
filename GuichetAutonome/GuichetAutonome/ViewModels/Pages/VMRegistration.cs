using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMRegistration : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The last name of the user
        /// </summary>
        [ObservableProperty]
        private string _lastName;

        /// <summary>
        /// The first name of the user
        /// </summary>
        [ObservableProperty]
        private string _firstName;

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

        /// <summary>
        /// The confirmation of the password
        /// </summary>
        [ObservableProperty]
        private string _confirmation;

        /// <summary>
        /// The phone number of the user
        /// </summary>
        [ObservableProperty]
        private string _phoneNumber;

        /// <summary>
        /// The city of the user
        /// </summary>
        [ObservableProperty]
        private string _city;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMRegistration()
        {
            InitializeProperties();
        }

        #endregion


        #region Commands

        /// <summary>
        /// The command to register the user
        /// </summary>
        [RelayCommand]
        public async Task Register()
        {
            // Change the page to the connection page
            VMMainWindow.Instance.ChangePage(typeof(Connection));
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initializes the properties
        /// </summary>
        private void InitializeProperties()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Confirmation = string.Empty;
            PhoneNumber = string.Empty;
            City = string.Empty;
        }

        #endregion
    }
}
