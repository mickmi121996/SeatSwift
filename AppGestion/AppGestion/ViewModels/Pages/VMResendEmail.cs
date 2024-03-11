using AppGestion.DataAccessLayer;
using AppGestion.Properties;
using AppGestion.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMResendEmail : ObservableObject
    {
        #region properties

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMResendEmail()
        {
            Initialize();
        }


        #endregion


        #region Commands

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

                        if(SelectedOrder.Client is not null)
                        {
                            // Send the email
                            await EmailTools.SendEmailWithSMTP2GO(SelectedOrder.Client.Email, emailSubjectFormat, emailContent);
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
            return true;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize the view model
        /// </summary>
        public void Initialize()
        {
            Orders = new ObservableCollection<Order>();
            SelectedOrder = new Order();
        }

        #endregion
    }
}
