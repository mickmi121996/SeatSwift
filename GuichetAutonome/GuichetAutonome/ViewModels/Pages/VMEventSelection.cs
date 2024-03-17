using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.ViewModels.UserControls;
using GuichetAutonome.Views.Pages;
using GuichetAutonome.Views.UserControls;
using SeatSwiftDLL;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMEventSelection : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Observable collection de UserControl
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserControl> _userControls;

        #endregion


        #region Constructor

        public VMEventSelection()
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            _userControls = new ObservableCollection<UserControl>();

            VMMainWindow.Instance.ResetInactivityTimer();

            Task.Run(() => InitializeUsersControl());
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to change page to the history page
        /// </summary>
        [RelayCommand]
        public void ChangePageToHistory()
        {
            VMMainWindow.Instance.ChangePage(typeof(HistoryShow));
        }

        /// <summary>
        /// Command to change page to the cart page
        /// </summary>
        [RelayCommand]
        public void ChangePageToCart()
        {
            VMMainWindow.Instance.ChangePage(typeof(CartShow));
        }

        /// <summary>
        /// Command to change page to the connection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToConnection()
        {
            VMMainWindow.Instance.LogoutUser();
        }

        /// <summary>
        /// Command to change page to the compte info page
        /// </summary>
        [RelayCommand]
        public void ChangePageToCompteInfo()
        {
            VMMainWindow.Instance.CurrentPage = new Registration(VMMainWindow.Instance.Client);
        }

        /// <summary>
        /// command to the seat selection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToSeatSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(SeatSelection));
        }

        #endregion


        #region Methods

        /// <summary>
        /// The method to initialize the list of users control.
        /// </summary>
        async private void InitializeUsersControl()
        {
            try
            {
                // Get show with incoming representation
                var shows = await DAL.ShowFactory.GetAllActiveWithIncomingRepresentationAsync();

                // Create a new list of show
                ObservableCollection<Show> showList = new ObservableCollection<Show>();

                // Loops through all the show
                foreach (Show var in shows)
                {
                    // Add the show to the list of show
                    showList.Add(var);
                }

                // Loops through all the show
                foreach (Show show in showList)
                {
                    // Adds a new ShowControl to the list of users control
                    Application.Current.Dispatcher.Invoke(
                        () => UserControls.Add(new EventTemplate(show))
                    );
                }

                // Changes the order of the list of users control
                Application.Current.Dispatcher.Invoke(
                    () =>
                        UserControls = new ObservableCollection<UserControl>(
                            UserControls.OrderBy(
                                uc => (uc.DataContext as VMEventTemplate)?.Show.Name
                            )
                        )
                );
            }
            catch (Exception e)
            {
                // Message box to display the error
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.CompletedTask;
        }

        #endregion
    }
}
