using AppGestion.DataAccessLayer;
using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using SeatSwiftDLL.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAddEditEmploye : ObservableObject
    {
        #region properties

        /// <summary>
        /// The user
        /// </summary>
        [ObservableProperty]
        private User _user;

        /// <summary>
        /// The is edit mode
        /// </summary>
        [ObservableProperty]
        private bool _isEditMode;

        /// <summary>
        /// The visibility of the password
        /// </summary>
        [ObservableProperty]
        private Visibility _passwordVisibility;

        /// <summary>
        /// The list of role
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _listRole;

        /// <summary>
        /// The selected role
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _selectedRole;

        /// <summary>
        /// The first name
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _firstName;

        /// <summary>
        /// The last name
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _lastName;

        /// <summary>
        /// The email
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _email;

        /// <summary>
        /// The password
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _password;

        /// <summary>
        /// The phone number
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _phoneNumber;

        /// <summary>
        /// The employee number
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _employeeNumber;

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
        /// Default constructor for add
        /// </summary>
        public VMAddEditEmploye()
        {
            IsEditMode = false;
            PasswordVisibility = Visibility.Visible;
            InitializeListRole();
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;
            _password = string.Empty;
            _phoneNumber = string.Empty;
            _employeeNumber = string.Empty;

            // Select the first role by default
            _selectedRole = ListRole.FirstOrDefault().ToString();

            _invalidEmailErrorMessageVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Constructor for edit
        /// </summary>
        /// <param name="user">The user to edit</param>
        public VMAddEditEmploye(User user)
        {
            User = user;
            IsEditMode = true;
            PasswordVisibility = Visibility.Collapsed;
            InitializeListRole();
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _email = user.Email;
            _password = "**********";
            _phoneNumber = user.PhoneNumber;
            _employeeNumber = user.EmployeeNumber;
            _selectedRole = user.Type.ToString();
            _invalidEmailErrorMessageVisibility = Visibility.Collapsed;
        }

        #endregion


        #region commands

        /// <summary>
        /// Command to add or edit the user
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanExecuteConfirm))]
        public async Task Confirm()
        {
            try
            {
                // Check if we are in edit mode
                if (IsEditMode)
                {
                    if(User is not  null)
                    {
                        // Edit the user
                        User.FirstName = FirstName;
                        User.LastName = LastName;
                        User.Email = Email;
                        User.PhoneNumber = PhoneNumber;
                        User.EmployeeNumber = EmployeeNumber;
                        User.Type = (EmployeeType)Enum.Parse(typeof(EmployeeType), SelectedRole);
                        await DAL.UserFactory.UpdateAsync(User);

                        // Close the window
                        Application.Current.Windows.OfType<AddEditEmploye>().FirstOrDefault()?.Close();
                    }
                    else
                    {
                        // SHow an error message box
                        MessageBox.Show("Aucun utilisateur sélectionné", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Create the user
                    User = new User
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        EmployeeNumber = EmployeeNumber,
                        Type = (EmployeeType)Enum.Parse(typeof(EmployeeType), SelectedRole),
                    };

                    // Add the user to the database
                    await DAL.UserFactory.CreateAsync(User);

                    // Create the salt and hash the password
                    await DAL.UserFactory.CreateSaltAndHashAsync(Password, User);       

                    // Close the window
                    Application.Current.Windows.OfType<AddEditEmploye>().FirstOrDefault()?.Close();   
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Command to check if the button confirm is enabled
        /// </summary>
        public bool CanExecuteConfirm()
        {
            if (
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(EmployeeNumber) ||
                string.IsNullOrWhiteSpace(SelectedRole) ||
                string.IsNullOrWhiteSpace(Password)
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

        #endregion


        #region methods

        /// <summary>
        /// Initialize the list of role
        /// </summary>
        public void InitializeListRole()
        {
            // Create the list of role and fill it with fictive data
            ListRole = new ObservableCollection<string>();
            ListRole.Add(EmployeeType.Accountant.ToString());
            ListRole.Add(EmployeeType.Administrator.ToString());
        }

        #endregion
    }
}
