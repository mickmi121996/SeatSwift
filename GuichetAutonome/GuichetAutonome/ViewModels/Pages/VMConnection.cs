using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.Views;
using GuichetAutonome.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMConnection : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The email of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _email;

        /// <summary>
        /// The password of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _password;

        /// <summary>
        /// The visibility of the error message
        /// </summary>
        [ObservableProperty]
        private Visibility _invalidEmailErrorMessageVisibility;

        /// <summary>
        /// The error message for invalid email
        /// </summary>
        [ObservableProperty]
        private string? _invalidEmailErrorMessage;

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
        [RelayCommand(CanExecute = nameof(CanConnect))]
        public async Task Connect()
        {
            try
            {
                // Check if the client exists
                SeatSwiftDLL.Client client = await DAL.ClientFactory.GetByEmailAsync(Email);

                // If th password is correct
                if (client != null && await DAL.ClientFactory.CheckPasswordAsync(Password, client))
                {
                    VMMainWindow.Instance.Client = client;

                    VMMainWindow.Instance.ChangePage(typeof(EventSelection));

                }
                else
                {
                    MessageBox.Show("le mot de passe est incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// fonction to check if the command can be executed
        /// </summary>
        /// <returns></returns>
        public bool CanConnect()
        {
            if (
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password)
            )
            {
                return false;
            }

            // Check if the email is valid
            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                InvalidEmailErrorMessage = "L'adresse email n'est pas valide";
                InvalidEmailErrorMessageVisibility = Visibility.Visible;
                return false;
            }
            InvalidEmailErrorMessage = string.Empty;
            InvalidEmailErrorMessageVisibility = Visibility.Collapsed;

            return true;
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
