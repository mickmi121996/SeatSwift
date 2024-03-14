using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views.Pages;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SeatSwiftDLL;
using GuichetAutonome.DataAccessLayer;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMCheckout : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The total price of the order
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

        /// <summary> 
        /// The first name on the credit card
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _firstName;

        /// <summary>
        /// The last name on the credit card
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _lastName;

        /// <summary>
        /// The credit card number
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _creditCardNumber;

        /// <summary>
        /// The expiration date of the credit card
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _expirationDate;

        /// <summary>
        /// The security code of the credit card
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _securityCode;

        /// <summary>
        /// The city of the credit card
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePageToThanksCommand))]
        private string _city;

        /// <summary>
        /// The Invalid credit card number message
        /// </summary>
        [ObservableProperty]
        private string _invalidCreditCardNumberMessage;

        /// <summary>
        /// The Invalid expiration date message
        /// </summary>
        [ObservableProperty]
        private string _invalidExpirationDateMessage;

        /// <summary>
        /// The Invalid security code message
        /// </summary>
        [ObservableProperty]
        private string _invalidSecurityCodeMessage;

        /// <summary>
        /// The invalid credit card message visibility
        /// </summary>
        [ObservableProperty]
        private Visibility _invalidCreditCardMessageVisibility;

        /// <summary>
        /// The invalid expiration date message visibility
        /// </summary>
        [ObservableProperty]
        private Visibility _invalidExpirationDateMessageVisibility;

        /// <summary>
        /// The invalid security code message visibility
        /// </summary>
        [ObservableProperty]
        private Visibility _invalidSecurityCodeMessageVisibility;

        /// <summary>
        /// The list of order 
        /// </summary>
        public List<Order> Orders { get; set; }

        #endregion


        #region Constructor

        public VMCheckout()
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            _totalPrice = 0;
            _tps = 0;
            _tvq = 0;
            _totalAmountAfterTaxe = 0;

            InitializeAmount();
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the thanks page
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanChangePageToThanks))]
        public async Task ChangePageToThanks()
        {
            // Create the orders for the ticket groups
            var ticketGroups = GroupTicketsByRepresentation();
            Orders = new List<Order>();

            try
            {
                foreach (var group in ticketGroups)
                {
                    string orderNumber;
                    
                    do
                    {
                        // Generate a random order number
                        orderNumber = GenerateRandomOrderNumber();

                    } while (!await DAL.OrderFactory.IsOrderNumberUniqueAsync(orderNumber));

                    // Calculate the total price of the group
                    decimal totalPrice = CalculateTotalPrice(group);

                    // Create the order
                    var order = new Order
                    {
                        IsActive = true,
                        OrderNumber = orderNumber,
                        OrderDate = DateTime.Now,
                        TotalPrice = totalPrice,
                        Client = VMMainWindow.Instance.Client
                    };

                    // Create the order in the database
                    await DAL.OrderFactory.CreateAsync(order);

                    // Get the order from the database
                    order = await DAL.OrderFactory.GetByOrderNumberAsync(orderNumber);

                    // Assign the tickets to the order
                    foreach (var ticket in group)
                    {
                        await DAL.TicketFactory.AsignToOrderAsync(ticket, order);
                    }

                    // Add the order to the list of orders
                    Orders.Add(order);
                }

                // Clear the cart
                VMMainWindow.Instance.Cart.Clear();

                // Change the page to the thanks page
                VMMainWindow.Instance.ChangePage(typeof(Thanks));
            }
            catch (Exception ex)
            {
                // Make a message box
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Can execute the command to the thanks page
        /// </summary>
        /// <returns></returns>
        private bool CanChangePageToThanks()
        {
            // Validation des champs vides
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(CreditCardNumber) ||
                string.IsNullOrWhiteSpace(ExpirationDate) ||
                string.IsNullOrWhiteSpace(SecurityCode) ||
                string.IsNullOrWhiteSpace(City))
            {
                return false;
            }

            // Validation du numéro de carte de crédit
            if (!Regex.IsMatch(CreditCardNumber, @"^\d{16}$"))
            {
                InvalidCreditCardNumberMessage = "Le numéro de carte de crédit est invalide";
                InvalidCreditCardMessageVisibility = Visibility.Visible;
                return false;
            }

            // Validation du code de sécurité
            if (!Regex.IsMatch(SecurityCode, @"^\d{3}$"))
            {
                InvalidSecurityCodeMessage = "Le code de sécurité est invalide";
                InvalidSecurityCodeMessageVisibility = Visibility.Visible;
                return false;
            }

            // Validation de la date d'expiration
            if (!Regex.IsMatch(ExpirationDate, @"^(0[1-9]|1[0-2])\/(\d{2})$"))
            {
                InvalidExpirationDateMessage = "La date d'expiration est invalide";
                InvalidExpirationDateMessageVisibility = Visibility.Visible;
                return false;
            }

            // Retirer les messages d'erreur
            InvalidCreditCardMessageVisibility = Visibility.Collapsed;
            InvalidExpirationDateMessageVisibility = Visibility.Collapsed;
            InvalidSecurityCodeMessageVisibility = Visibility.Collapsed;

            // Toutes les validations passent
            return true;
        }

        /// <summary>
        /// Command to the cart page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToCart()
        {
            VMMainWindow.Instance.ChangePage(typeof(CartShow));
        }

        #endregion


        #region Methods

        /// <summary>
        /// The method to get the list of 

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

        /// <summary>
        /// The method to group the tickets by representation
        /// </summary>
        private List<List<Ticket>> GroupTicketsByRepresentation()
        {
            var groupedTickets = VMMainWindow.Instance.Cart
                .GroupBy(ticket => ticket.Representation.Id)
                .Select(group => group.ToList())
                .ToList();

            return groupedTickets;
        }

        /// <summary>
        /// The method to generate the random order number
        /// </summary>
        /// <returns>The random order number</returns>
        private string GenerateRandomOrderNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// The method to calculate the total price of the group
        /// </summary>
        /// <param name="group">The group of tickets</param>
        /// <returns>The total price of the group</returns>
        private decimal CalculateTotalPrice(List<Ticket> group)
        {
            return group.Sum(ticket => ticket.Representation.Show.BasePrice);
        }

        #endregion
    }
}
