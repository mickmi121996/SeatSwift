using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMHome : ObservableObject
    {
        #region properties

        /// <summary>
        /// The first name of the user
        /// </summary>
        [ObservableProperty]
        private string _firstName;

        /// <summary>
        /// The last name of the user
        /// </summary>
        [ObservableProperty]
        private string _lastName;

        /// <summary>
        /// The email of the user
        /// </summary>
        [ObservableProperty]
        private string _email;

        /// <summary>
        /// The Employee Number of the user
        /// </summary>
        [ObservableProperty]
        private string _employeeNumber;

        /// <summary>
        /// The user's role
        /// </summary>
        [ObservableProperty]
        private string _role;

        /// <summary>
        /// The user's phone number
        /// </summary>
        [ObservableProperty]
        private string _phoneNumber;

        /// <summary>
        /// The number of Shows created by the user
        /// </summary>
        [ObservableProperty]
        private int _showsCreated;

        /// <summary>
        /// The number of representations for the selected show
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedShow))]
        private int _representations;

        /// <summary>
        /// The list of Shows created by the user
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Show> _shows;

        /// <summary>
        /// The selected show
        /// </summary>
        [ObservableProperty]
        private Show? _selectedShow;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMHome()
        {
            InitializeProperties();
            UpdatePage();
        }

        #endregion


        #region commands



        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            EmployeeNumber = string.Empty;
            Role = string.Empty;
            PhoneNumber = string.Empty;
            Shows = new ObservableCollection<Show>();
            SelectedShow = null;
            ShowsCreated = 0;
            Representations = 0;
        }

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private async void UpdatePage()
        {
            FirstName = VMMainWindow.Instance.User.FirstName;
            LastName = VMMainWindow.Instance.User.LastName;
            Email = VMMainWindow.Instance.User.Email;
            EmployeeNumber = VMMainWindow.Instance.User.EmployeeNumber;
            Role = VMMainWindow.Instance.User.Type.ToString();
            PhoneNumber = VMMainWindow.Instance.User.PhoneNumber;

            var shows = await DAL.ShowFactory.GetAllActiveByUserIdAsync(VMMainWindow.Instance.User.Id);
            Shows.Clear();
            foreach (var show in shows)
            {
                Shows.Add(show);
            }

            ShowsCreated = Shows.Count;

            UpdateRepresentations();
        }

        /// <summary>
        /// Update the number of representations for the selected show
        /// </summary>
        private async void UpdateRepresentations()
        {
            if (SelectedShow != null)
            {
                var representations = await DAL.RepresentationFactory.GetInComingActiveAvailableByShowAsync(SelectedShow);
                Representations = representations.Count;
            }
        }

        /// <summary>
        /// Method called when the selected show changes
        /// </summary>
        partial void OnSelectedShowChanged(Show value)
        {
            // Call the method to update representations when the selected show changes.
            UpdateRepresentations();
        }

        #endregion
    }
}
