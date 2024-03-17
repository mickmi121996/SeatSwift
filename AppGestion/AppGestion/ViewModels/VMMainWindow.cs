using CommunityToolkit.Mvvm.ComponentModel;
using SeatSwiftDLL;
using System.Windows;

namespace AppGestion.ViewModels
{
    public partial class VMMainWindow : ObservableObject
    {
        #region Instance

        /// <summary>
        /// The instance of the MainWindowVM
        /// </summary>
        public static VMMainWindow Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new VMMainWindow(new User());
                }
                return _instance;
            }
        }

        /// <summary>
        /// Backing field for the MainWindowVM instance
        /// </summary>
        private static VMMainWindow? _instance;

        #endregion


        #region Properties

        /// <summary>
        /// The title of the current page
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// The is creating ticket
        /// </summary>
        [ObservableProperty]
        private Visibility _isCurrentlyWorking;

        /// <summary>
        /// The the connection status of the user
        /// </summary>
        [ObservableProperty]
        private bool _isConnected;

        /// <summary>
        /// The user currently connected
        /// </summary>
        [ObservableProperty]
        private User _user;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMMainWindow(User user)
        {
            _isCurrentlyWorking = Visibility.Collapsed;
            _title = string.Empty;
            _isConnected = false;
            _user = new User();
        }

        #endregion


        #region Commands



        #endregion


        #region Methods

        public void ChangeUser(User user)
        {
            User = user;

            IsConnected = true;

            // Check the employee type of the user
            if (User.Type == SeatSwiftDLL.Enums.EmployeeType.Administrator) { }
            else { }
        }

        #endregion
    }
}
