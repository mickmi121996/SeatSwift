using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuichetAutonome.ViewModels.UserControls
{
    public partial class VMOrderTemplate : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The order
        /// </summary>
        [ObservableProperty]
        private Order _order;

        /// <summary>
        /// Order number
        /// </summary>
        [ObservableProperty]
        private string _orderNumber;

        /// <summary>
        /// Observable collection of tickets
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Ticket> _tickets;

        /// <summary>
        /// The show of the order
        /// </summary>
        [ObservableProperty]
        private Show _show;

        /// <summary>
        /// The total price of the order
        /// </summary>
        [ObservableProperty]
        private decimal _totalPrice;


        /// <summary>
        /// Number of tickets
        /// </summary>
        [ObservableProperty]
        private int _numberOfTickets;

        #endregion


        #region Constructor

        /// <summary>
        /// The constructor of the VMOrderTemplate
        /// </summary>
        public VMOrderTemplate(List<Ticket> tickets)
        {
            // Initialize the view model
            _show = new Show();
            _tickets = new ObservableCollection<Ticket>();
            _order = new Order();
            _totalPrice = 0;
            _numberOfTickets = 0;

            //Generate a random number
            var random = new Random();
            _orderNumber = random.Next(100000, 999999).ToString();

            // Create the list of tickets
            var ticketToAdd = tickets;
            // Add the ticket to the collection
            foreach (var ticket in ticketToAdd)
            {
                _tickets.Add(ticket);

                // Increment the number of tickets
                _numberOfTickets++;

                // Add the price of the ticket to the total price
                if(ticket.Representation is not null)
                {
                    if(ticket.Representation.Show is not null)
                    {
                        _totalPrice += ticket.Representation.Show.BasePrice;
                        _show = ticket.Representation.Show;
                    }
                }                
            }

            CreateOrder();
        }

        #endregion


        #region Command

        /// <summary>
        /// Delete the ticket of the cart in VMMainWindow
        /// </summary>
        [RelayCommand]
        public async Task DeleteTicket()
        {
            try
            {
                // Check for each ticket in the cart with the same id
                foreach (var ticket in VMMainWindow.Instance.Cart)
                {
                    // Check if the ticket is the same as one of the ticket in the _tickets collection
                    if (_tickets.Contains(ticket))
                    {
                        // Remove the ticket from the cart
                        VMMainWindow.Instance.Cart.Remove(ticket);

                        // Remove the ticket from the collection
                        _tickets.Remove(ticket);
                    }    
                }
            }
            catch (Exception ex)
            {
                // Make a message box
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }  
        }

        #endregion


        #region Methods

        /// <summary>
        /// Create an order
        /// </summary>
        /// <returns>The order</returns>
        /// <exception cref="Exception">If the order is not created</exception>
        /// <exception cref="Exception">If the tickets are not added to the order</exception>
        public async void CreateOrder()
        {
            try
            {
                //Check if the order number is unique in the database
                while (!await DAL.OrderFactory.IsOrderNumberUniqueAsync(OrderNumber))
                {
                    // Generate a random number
                    var random = new Random();
                    OrderNumber = random.Next(100000, 999999).ToString();
                }

                // Create the order with the order number, the date, the total price and the client
                Order = new Order
                {
                    OrderNumber = OrderNumber,
                    OrderDate = DateTime.Now,
                    TotalPrice = TotalPrice,
                    Client = VMMainWindow.Instance.Client
                };
            }
            catch (Exception ex)
            {
                // Make a message box
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
