using AppGestion.DataAccessLayer;
using AppGestion.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMConnection : ObservableObject
    {
        #region properties

        /// <summary>
        /// The Employee number of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _employeeNumber;

        /// <summary>
        /// The password of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _password;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMConnection()
        {
            _ = InitializeProperties();
        }

        #endregion


        #region commands

        /// <summary>
        /// The command to connect the user
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanExecuteConnect))]
        public async Task Connect()
        {
            try
            {
                // Check if the user exist
                User user = await DAL.UserFactory.GetByEmployeeNumberAsync(EmployeeNumber);

                // Check if the password is correct
                if (user != null && await DAL.UserFactory.CheckPasswordAsync(Password, user))
                {
                    // Open the main window
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();

                    // Close the connection window
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("The password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Afficher le message d'erreur pour toute autre exception inattendue
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Check if the user can connect
        /// </summary>
        /// <returns>True if the user can connect</returns>
        public bool CanExecuteConnect()
        {
            return !string.IsNullOrEmpty(EmployeeNumber) && !string.IsNullOrEmpty(Password);
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private async Task InitializeProperties()
        {
            //Check the count of user in the database
            if (await DAL.UserFactory.CountActiveAsync() == 0)
            {
                // Create a default admin user
                await CreateAdminUser();
            }
            EmployeeNumber = string.Empty;
            Password = string.Empty;
        }

        #endregion

        #region Fake user

        /// <summary>
        /// This method is use to create a default admin user if the count of user in the data base is 0
        /// </summary>
        public async Task CreateAdminUser()
        {
            // Create a new user using the default constructor
            User user = new User()
            {
                EmployeeNumber = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "mickmi12@hotmail.com",
                PhoneNumber = "418-944-1603",
                Type = EmployeeType.Administrator,
            };

            // add the user to the database
            await DAL.UserFactory.CreateAsync(user);

            // Set the salt and the hash of the password
            await DAL.UserFactory.CreateSaltAndHashAsync("admin",user);
        }

        #endregion
    }
}
