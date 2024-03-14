using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.ViewModels.UserControls;
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
    public partial class VMCartShow : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Observable collection de UserControl
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserControl> _userControls;

        /// <summary>
        /// The total price of the cart
        /// </summary>
        [ObservableProperty]
        private decimal _totalPrice;

        /// <summary>
        /// The TPS amount
        /// </summary>
        [ObservableProperty]
        private decimal _tps;

        /// <summary>
        /// The TVQ amount
        /// </summary>
        [ObservableProperty]
        private decimal _tvq;

        /// <summary>
        /// The total amount of the transaction after taxes
        /// </summary>
        [ObservableProperty]
        private decimal _totalAmountAfterTaxe;

        #endregion


        #region Constructor

        public VMCartShow()
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            // Initialize the view model
            _userControls = new ObservableCollection<UserControl>();
            _totalPrice = 0;
            _tps = 0;
            _tvq = 0;
            _totalAmountAfterTaxe = 0;

            InitializeUsersControl();

            InitializeAmount();
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the checkout page
        /// </summary>
        [RelayCommand]
        public void ChangePageToCheckout()
        {
            VMMainWindow.Instance.ChangePage(typeof(Checkout));
        }

        /// <summary>
        /// Command to the event selection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToEventSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
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
                var ticketsGroupedByRepresentation = VMMainWindow.Instance.Cart
                    .GroupBy(ticket => ticket.Representation.Id)
                    .ToList();

                foreach (var group in ticketsGroupedByRepresentation)
                {
                    List<Ticket> ticketsForCurrentRepresentation = group.ToList();

                    OrderTemplate orderTemplateForCurrentRepresentation = new OrderTemplate(ticketsForCurrentRepresentation);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UserControls.Add(orderTemplateForCurrentRepresentation);
                    });
                }

                // Changes the order of the list of users control
                Application.Current.Dispatcher.Invoke(
                    () =>
                        UserControls = new ObservableCollection<UserControl>(
                            UserControls
                                .OrderBy(
                                    uc =>
                                        (uc.DataContext as VMOrderTemplate)?.Show.Name
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
        
        /// <summary>
        /// The method to calculate the total price of the cart
        /// </summary>
        private void InitializeAmount()
        {
            // Reset the total price
            _totalPrice = 0;

            // Calculate the total price
            foreach (var ticket in VMMainWindow.Instance.Cart)
            {
                if (ticket.Representation is not null)
                {
                    if (ticket.Representation.Show is not null)
                    {
                        _totalPrice += ticket.Representation.Show.BasePrice;
                    }
                }
            }

            // Calculate the TPS amount
            _tps = TpsCalculation(_totalPrice);

            // Calculate the TVQ amount
            _tvq = TvqCalculation(_totalPrice);

            // Calculate the total amount after taxes
            _totalAmountAfterTaxe = TotalAmountAfterTaxeCalculation(_totalPrice, _tps, _tvq);
        }

        /// <summary>
        /// The tps calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tps amount</returns>
        private decimal TpsCalculation(decimal totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.05m;
        }

        /// <summary>
        /// The tvq calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <returns>The tvq amount</returns>
        private decimal TvqCalculation(decimal totalAmountBeforeTaxe)
        {
            return totalAmountBeforeTaxe * 0.09975m;
        }

        /// <summary>
        /// The total amount after taxes calculation
        /// </summary>
        /// <param name="totalAmountBeforeTaxe">The total amount before taxes</param>
        /// <param name="tps">The tps amount</param>
        /// <param name="tvq">The tvq amount</param>
        /// <returns>The total amount after taxes</returns>
        private decimal TotalAmountAfterTaxeCalculation(decimal totalAmountBeforeTaxe, decimal tps, decimal tvq)
        {
            return totalAmountBeforeTaxe + tps + tvq;
        }

        #endregion
    }
}
