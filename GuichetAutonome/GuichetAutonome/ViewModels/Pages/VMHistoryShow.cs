using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.DataAccessLayer;
using GuichetAutonome.Properties;
using GuichetAutonome.Tools;
using GuichetAutonome.Views.Pages;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMHistoryShow : ObservableObject
    {
        #region Properties

        /// <summary>
        /// A ObservableCollection of Order
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Order> _orders;

        /// <summary>
        /// The selected order
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResendEmailCommand))]
        private Order _selectedOrder;

        #endregion


        #region Constructor

        public VMHistoryShow()
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            // Initialize the view model
            _orders = new ObservableCollection<Order>();
            _selectedOrder = new Order();

            Initialize();
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the event selection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToEventSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        /// <summary>
        /// The command to resend the email
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanResendEmail))]
        public async Task ResendEmail()
        {
            try
            {
                if (SelectedOrder is not null)
                {
                    // Get the tickets of the order
                    List<Ticket> tickets = await DAL.TicketFactory.GetByOrderAsync(SelectedOrder);
                    if (tickets is not null)
                    {
                        // Create a StringBuilder to store the QR codes
                        StringBuilder qrCodesHtml = new StringBuilder();
                        foreach (Ticket ticket in tickets)
                        {
                            // Generate the QR code
                            string qrCodeBase64 = CodeQRTools.GenerateQRCodeBase64(ticket.QRCodeData);

                            // Add the QR code to the StringBuilder
                            qrCodesHtml.AppendFormat("<img src=\"{0}\" style=\"width: 270px; height: 270px;\" alt=\"QR Code\" /><br/>", qrCodeBase64);
                        }

                        // Get the email template
                        string emailTemplate = Resources.EmailCore;
                        string emailContent = string.Format(emailTemplate, SelectedOrder.OrderNumber, qrCodesHtml.ToString());

                        string emailSubject = Resources.ResendEmailSubject;
                        string emailSubjectFormat = string.Format(emailSubject, SelectedOrder.OrderNumber);

                        if (SelectedOrder.Client is not null)
                        {
                            // Send the email
                            await EmailTools.SendEmailWithSMTP2GO(VMMainWindow.Instance.Client.Email, emailSubjectFormat, emailContent);
                        }
                        else
                        {
                            MessageBox.Show("The client of the order is null", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// The command to check if the resend email command can be executed
        /// </summary>
        public bool CanResendEmail()
        {
            if(SelectedOrder is not null)
            {
                return true;
            }
            return false;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize the view model
        /// </summary>
        public async void Initialize()
        {
            var orders = await DAL.OrderFactory.GetByClientIdAsync(VMMainWindow.Instance.Client.Id);

            // Clear the list of orders
            Orders.Clear();

            // Add the orders to the list
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            
            // If there is at least one order
            if (Orders.Count > 0)
            {
                // Select the first order
                SelectedOrder = Orders[0];
            }
        }

        #endregion
    }
}
