using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.ViewModels.Pages;
using GuichetAutonome.Views;
using GuichetAutonome.Views.Pages;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GuichetAutonome.ViewModels
{
    public partial class VMMainWindow : ObservableObject
    {

        /// <summary>
        /// The instance of the MainWindowVM
        /// </summary>
        public static VMMainWindow Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new VMMainWindow();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Backing field for the MainWindowVM instance
        /// </summary>
        private static VMMainWindow? _instance;

        private DispatcherTimer _inactivityTimer;

        #region Properties

        /// <summary>
        /// The current page that is being displayed
        /// </summary>
        [ObservableProperty]
        private Page _currentPage;

        /// <summary>
        /// The the connection status of the user
        /// </summary>
        [ObservableProperty]
        private bool _isConnected;

        /// <summary>
        /// The user currently connected
        /// </summary>
        [ObservableProperty]
        private Client _client;

        /// <summary>
        /// A list of order as a cart
        /// </summary>
        [ObservableProperty]
        private List<Ticket> _cart;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMMainWindow()
        {
            InitializeProperties();

            SetupInactivityTimer();
        }

        #endregion


        #region Commands



        #endregion


        #region Methods

        /// <summary>
        /// Changes the current page to the page name given
        /// </summary>
        /// <param name="pageName">The name of the page to change to</param>
        [RelayCommand]
        public void ChangePage(Type pageType)
        {
            if (pageType == typeof(Connection))
            {
                CurrentPage = new Connection();
            }
            else if (pageType == typeof(Registration))
            {
                CurrentPage = new Registration();
            }
            else if(pageType == typeof(EventSelection)) 
            {
                CurrentPage = new EventSelection();
            }
            else if (pageType == typeof(HistoryShow))
            {
                CurrentPage = new HistoryShow();
            }
            else if (pageType == typeof(Checkout))
            {
                CurrentPage = new Checkout();
            }
            else if (pageType == typeof(CartShow))
            {
                CurrentPage = new CartShow();
            }
            else if (pageType == typeof(Thanks))
            {
                CurrentPage = new Thanks();
            }
        }

        private void SetupInactivityTimer()
        {
            _inactivityTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(5)
            };

            _inactivityTimer.Tick += OnInactivityTimerExpired;
            _inactivityTimer.Start();
        }

        public void ResetInactivityTimer()
        {
            _inactivityTimer?.Stop();
            _inactivityTimer?.Start();
        }

        private void OnInactivityTimerExpired(object? sender, EventArgs e)
        {
            LogoutUser();
        }

        public async Task LogoutUser()
        {
            if (Cart != null && Cart.Count > 0)
            {
                foreach (var ticket in Cart)
                {
                    await DAL.TicketFactory.MakeAvailableAsync(ticket);
                }
                Cart.Clear(); 
            }

            IsConnected = false;
            Client = null;

            ChangePage(typeof(Connection));

            ResetInactivityTimer();
        }


        public void ChangeUser(Client client)
        {
            Client = client;

            IsConnected = true;
        }

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            Client = new Client();
            IsConnected = false;
            CurrentPage = new Connection();
            Cart = new List<Ticket>();
        }

        #endregion
    }
}
