using AppGestion.DataAccessLayer;
using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMManageEmploye : ObservableObject
    {
        #region properties

        /// <summary>
        /// The list of users
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<User> _users;

        /// <summary>
        /// The selected user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenEditEmployeCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenEditEmployeCommand))]
        private User _selectedUser;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMManageEmploye()
        {
            InitializeProperties();
            LoadData();
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

            // Reload the data
            LoadData();
        }

        /// <summary>
        /// Command to open the EditEmploye window
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanOpenEditEmploye))]
        public void OpenEditEmploye()
        {
            // Open the AddEditEmploye window
            AddEditEmploye Editwindow = new AddEditEmploye(SelectedUser);
            Editwindow.ShowDialog();

            // Reload the data
            LoadData();
        }

        /// <summary>
        /// Check if the OpenEditEmploye command can be executed
        /// </summary>
        /// <returns>True if the command can be executed, false otherwise</returns>
        public bool CanOpenEditEmploye()
        {
            return SelectedUser != null;
        }

        /// <summary>
        /// Command to delete the selected user
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanDeleteUser))]
        public async Task DeleteUser()
        {
            // try to delete the user
            try
            {
                // Ask for confirmation
                MessageBoxResult result = MessageBox.Show(
                    "Voulez-vous vraiment supprimer cet employé ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                // if the user doesn't want to delete the user, return
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                // Delete the user from the database
                await DAL.UserFactory.SetAsInactiveAsync(SelectedUser.EmployeeNumber);

                // Remove the user from the list
                Users.Remove(SelectedUser);

                // if the list is not empty, select the first user
                if (Users.Count > 0)
                {
                    SelectedUser = Users[0];
                }
                else
                {
                    SelectedUser = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Check if the DeleteUser command can be executed
        /// </summary>
        /// <returns>True if the command can be executed, false otherwise</returns>
        public bool CanDeleteUser()
        {
            return SelectedUser != null;
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties to null or empty
        /// </summary>
        private void InitializeProperties()
        {
            Users = new ObservableCollection<User>();
            SelectedUser = new User();
        }

        /// <summary>
        /// Load the users from the database
        /// </summary>
        public async void LoadData()
        {
            try
            {
                // Get the users from the database
                var users = await DAL.UserFactory.GetAllActiveAsync();

                // Clear the list of users
                Users.Clear();

                // Add the users to the list
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                // if the list is not empty, select the first user
                if (Users.Count > 0)
                {
                    SelectedUser = Users[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
