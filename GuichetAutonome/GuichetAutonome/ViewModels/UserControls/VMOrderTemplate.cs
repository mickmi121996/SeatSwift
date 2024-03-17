using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.Views.Pages;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        #endregion


        #region Command

        /// <summary>
        /// Delete the ticket of the cart in VMMainWindow
        /// </summary>
        /// <summary>
        /// Delete the ticket from the cart in VMMainWindow.
        /// </summary>
        [RelayCommand]
        public async Task DeleteTicket()
        {
            try
            {
                // Create a list to hold tickets to be removed
                List<Ticket> ticketsToRemove = new List<Ticket>();

                // Identify the tickets to be removed
                foreach (var ticket in VMMainWindow.Instance.Cart)
                {
                    if (_tickets.Contains(ticket))
                    {
                        ticketsToRemove.Add(ticket);
                    }
                }

                // Remove the tickets from the cart and the collection
                foreach (var ticket in ticketsToRemove)
                {
                    VMMainWindow.Instance.Cart.Remove(ticket);
                    _tickets.Remove(ticket);
                    await DAL.TicketFactory.MakeAvailableAsync(ticket);
                }

                VMMainWindow.Instance.ChangePage(typeof(CartShow));
            }
            catch (Exception ex)
            {
                // Display a message box with the error
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task InitializeTicketsAsync(List<Ticket> tickets)
        {
            foreach (var ticket in tickets)
            {
                var ticket1 = await GiveTicketObject(ticket);
                if (ticket1 != null)
                {
                    Tickets.Add(ticket1);
                    NumberOfTickets++;
                    if (ticket1.Representation?.Show != null)
                    {
                        TotalPrice += ticket1.Representation.Show.BasePrice;
                        Show = ticket1.Representation.Show;
                    }
                }
            }

            CreateOrder();
        }

        public async Task<Ticket> GiveTicketObject(Ticket ticket)
        {
            try
            {
                ticket.Seat = await DAL.SeatFactory.GetByIdAsync(ticket.SeatId);
                ticket.Representation = await DAL.RepresentationFactory.GetByIdAsync(
                    ticket.RepresentationId
                );

                return ticket;
            }
            catch (Exception ex)
            {
                // Make a message box
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
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
