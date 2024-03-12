using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.Views.Pages;
using GuichetAutonome.Views.UserControls;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            _userControls = new ObservableCollection<UserControl>();
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
            VMMainWindow.Instance.ChangePage(typeof(Connection));
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
