using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
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
    public partial class VMRegistration : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The bool to check if we modify
        /// </summary>
        [ObservableProperty]
        private bool _isModifyState;

        /// <summary>
        /// The isRegistrationState
        /// </summary>
        [ObservableProperty]
        private Visibility _isRegistrationState;

        /// <summary>
        /// The visibility of ModifyState
        /// </summary>
        [ObservableProperty]
        private Visibility _modifyStateVisibility;

        /// <summary>
        /// The visibility of the password
        /// </summary>
        [ObservableProperty]
        private Visibility _passwordVisibility;

        /// <summary>
        /// The last name of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _lastName;

        /// <summary>
        /// The first name of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _firstName;

        /// <summary>
        /// The email of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _email;

        /// <summary>
        /// The password of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _password;

        /// <summary>
        /// The confirmation of the password
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _confirmation;

        /// <summary>
        /// The phone number of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _phoneNumber;

        /// <summary>
        /// The city of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private string _city;

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

        /// <summary>
        /// The client
        /// </summary>
        [ObservableProperty]
        private SeatSwiftDLL.Client _client;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMRegistration()
        {
            _client = new SeatSwiftDLL.Client();
            _isModifyState = false;
            _modifyStateVisibility = Visibility.Collapsed;
            _isRegistrationState = Visibility.Visible;
            _passwordVisibility = Visibility.Visible;
            InitializeProperties();
        }

        /// <summary>
        /// Default constructor for modifie
        /// </summary>
        public VMRegistration(SeatSwiftDLL.Client client)
        {
            _client = client;
            _isModifyState = true;
            _modifyStateVisibility = Visibility.Visible;
            _isRegistrationState = Visibility.Collapsed;
            _passwordVisibility = Visibility.Collapsed;
            _password = "********";
            _confirmation = "********";
            LastName = client.LastName;
            FirstName = client.FirstName;
            Email = client.Email;
            PhoneNumber = client.PhoneNumber;
            City = client.City;
        }

        #endregion


        #region Commands

        /// <summary>
        /// The command to register the user
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanRegister))]
        public async Task Register()
        {
            try
            {
                if (IsModifyState)
                {
                    // Update the client
                    Client = new SeatSwiftDLL.Client
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        City = City
                    };

                    await DAL.ClientFactory.UpdateAsync(Client);
                    VMMainWindow.Instance.ChangePage(typeof(EventSelection));
                }
                else
                {
                    // Create the client
                    Client = new SeatSwiftDLL.Client
                    {
                        LastName = LastName,
                        FirstName = FirstName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        City = City
                    };

                    await DAL.ClientFactory.CreateAsync(Client);

                    Client = await DAL.ClientFactory.GetByEmailAsync(Client.Email);

                    await DAL.ClientFactory.CreateSaltAndHashAsync(Password, Client);

                    // Change the page to the connection page
                    VMMainWindow.Instance.ChangePage(typeof(Connection));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Command to change the page to the connection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToConnection()
        {
            VMMainWindow.Instance.ChangePage(typeof(Connection));
        }

        /// <summary>
        /// Command to change the page to the Selection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        /// <summary>
        /// The can register command
        /// </summary>
        /// <returns></returns>
        private bool CanRegister()
        {
            if (
                string.IsNullOrEmpty(LastName) ||
                string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Confirmation)
            )
            {
                return false;
            }

            // check if the password and the confirmation are the same
            if (Password != Confirmation)
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
