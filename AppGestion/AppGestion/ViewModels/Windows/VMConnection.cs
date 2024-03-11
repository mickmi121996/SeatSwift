using AppGestion.DataAccessLayer;
using AppGestion.Properties;
using AppGestion.Tools;
using AppGestion.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMConnection : ObservableObject
    {
        #region properties

        /// <summary>
        /// The Employee number of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _employeeNumber;

        /// <summary>
        /// The password of the user
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private string _password;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMConnection()
        {
            _ = InitializeProperties();
        }

        #endregion


        #region commands

        /// <summary>
        /// The command to connect the user
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanExecuteConnect))]
        public async Task Connect()
        {
            try
            {
                // Check if the user exist
                User user = await DAL.UserFactory.GetByEmployeeNumberAsync(EmployeeNumber);

                // Check if the password is correct
                if (user != null && await DAL.UserFactory.CheckPasswordAsync(Password, user))
                {
                    // Open the main window
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();

                    // Close the connection window
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("The password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Afficher le message d'erreur pour toute autre exception inattendue
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Create a fake setup to try to send an email
        /// </summary>
        //[RelayCommand(CanExecute = nameof(CanExecuteConnect))]
        //public async Task Connect()
        //{
        //    // Créer un utilisateur
        //    User user = new User(1, true, "Jean", "Dupont", "JD123", EmployeeType.Administrator, "jean.dupont@example.com", "0123456789");

        //    // Créer un spectacle
        //    Show show = new Show(1, true, "Le Spectacle Fantastique", "Les Artistes Incroyables", "Une description captivante", ShowType.Theater, "urlImage.jpg", 5, 20.0m, user);

        //    // Créer une salle de spectacle
        //    Auditorium auditorium = new Auditorium(1, true, "Grand Théâtre", 20, 50);

        //    // Créer une représentation
        //    Representation representation = new Representation(1, true, DateTime.Now, RepresentationStatus.Available, show, auditorium);

        //    // Créer des sièges
        //    Seat seat1 = new Seat(1, 1, SeatStatus.InService, auditorium, SectionName.Balcon, 10, 20);
        //    Seat seat2 = new Seat(2, 2, SeatStatus.InService, auditorium, SectionName.Balcon, 10, 21);

        //    // Créer un client
        //    Client client = new Client(1, true, "Marie", "Leroux", "mickmi12@hotmail.com", "0987654321", "Paris");

        //    // Créer une commande
        //    Order order = new Order(1, true, "ORD12345", DateTime.Now, 40.0m, client);

        //    // Créer des billets
        //    Ticket ticket1 = new Ticket(1, true, "RES123", TicketStatus.Purchased, representation, seat1, order);
        //    Ticket ticket2 = new Ticket(2, true, "RES124", TicketStatus.Purchased, representation, seat2, order);

        //    // Ajouter les billets à une liste
        //    List<Ticket> tickets = new List<Ticket> { ticket1, ticket2 };

        //    try
        //    {
        //        if (tickets is not null)
        //        {
        //            StringBuilder qrCodesHtml = new StringBuilder();
        //            foreach (Ticket ticket in tickets)
        //            {
        //                string qrCodeBase64 = CodeQRTools.GenerateQRCodeBase64(ticket.QRCodeData);
        //                qrCodesHtml.AppendFormat("<img src=\"{0}\" style=\"width: 275px; height: 275px;\" alt=\"QR Code\" /><br/>", qrCodeBase64);
        //            }


        //            string emailTemplate = Resources.EmailCore;
        //            string emailContent = string.Format(emailTemplate, order.OrderNumber, qrCodesHtml.ToString());


        //            await EmailTools.SendEmailWithSMTP2GO(order.Client.Email, "Votre commande SeatSwift", emailContent);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gérer l'exception
        //        Console.WriteLine($"Une erreur s'est produite: {ex.Message}");
        //    }
        //}


        /// <summary>
        /// Check if the user can connect
        /// </summary>
        /// <returns>True if the user can connect</returns>
        public bool CanExecuteConnect()
        {
            return !string.IsNullOrEmpty(EmployeeNumber) && !string.IsNullOrEmpty(Password);
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private async Task InitializeProperties()
        {
            //Check the count of user in the database
            if (await DAL.UserFactory.CountActiveAsync() == 0)
            {
                // Create a default admin user
                await CreateAdminUser();
            }
            EmployeeNumber = string.Empty;
            Password = string.Empty;
        }

        #endregion

        #region Fake user

        /// <summary>
        /// This method is use to create a default admin user if the count of user in the data base is 0
        /// </summary>
        public async Task CreateAdminUser()
        {
            // Create a new user using the default constructor
            User user = new User()
            {
                EmployeeNumber = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "mickmi12@hotmail.com",
                PhoneNumber = "418-944-1603",
                Type = EmployeeType.Administrator,
            };

            // add the user to the database
            await DAL.UserFactory.CreateAsync(user);

            // Set the salt and the hash of the password
            await DAL.UserFactory.CreateSaltAndHashAsync("admin",user);
        }

        #endregion
    }
}
