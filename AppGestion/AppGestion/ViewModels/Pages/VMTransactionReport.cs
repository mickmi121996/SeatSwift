using AppGestion.Classes;
using AppGestion.DataAccessLayer;
using AppGestion.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeatSwiftDLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMTransactionReport : ObservableObject
    {
        #region properties

        /// <summary>
        /// The selected date
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveReportCommand))]
        private DateTime _selectedDate;

        /// <summary>
        /// The filter list
        /// </summary>
        [ObservableProperty]
        private List<string> _filterList;

        /// <summary>
        /// The selected filter
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveReportCommand))]
        private string _selectedFilter;

        /// <summary>
        /// The list of orders
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedFilter))]
        [NotifyPropertyChangedFor(nameof(SelectedDate))]
        private ObservableCollection<Order> _orders;

        /// <summary>
        /// The list of sell lines
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<TransactionLine> _transactionLines;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMTransactionReport()
        {
            InitializeProperties();
            CreateTransactionLines();
        }

        #endregion


        #region commands

        /// <summary>
        /// Open the file exporer to save the report
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanExecuteExport))]
        public void SaveReport()
        {
            // Open the file explorer
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save the report";
            saveFileDialog.ShowDialog();

            // If the user selected a path
            if (saveFileDialog.FileName != "")
            {
                // Create the report
                PDFTools.CreateTransactionsReportPdf(saveFileDialog.FileName, TransactionLines.ToList(), SelectedDate, SelectedFilter);
            }
        }

        /// <summary>
        /// Method to check if the command can be executed
        /// </summary>
        private bool CanExecuteExport()
        {
            // if there is at least one sell line
            if (TransactionLines.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


        #region methods

        // /// <summary>
        // /// Method to create the transaction lines for the selected orders
        // /// </summary>
        // public async Task CreateTransactionLines()
        // {
        //     TransactionLines.Clear();

        //     foreach (var order in Orders)
        //     {
        //         var tickets = await DAL.TicketFactory.GetByOrderAsync(order);
        //         if (tickets.Count > 0)
        //         {
        //             // Calculer le nombre total de tickets et le montant total avant taxes pour cette commande
        //             int ticketsSold = tickets.Count;
        //             double totalAmountBeforeTaxe = tickets.Sum(ticket => (double)ticket.Representation.Show.BasePrice);

        //             // Créer une nouvelle ligne de transaction pour cette commande
        //             var transactionLine = new TransactionLine(
        //                 order.OrderNumber,
        //                 order.OrderDate,
        //                 totalAmountBeforeTaxe,
        //                 order.Client.Email,
        //                 ticketsSold
        //             );

        //             // Ajouter la ligne de transaction à la liste
        //             TransactionLines.Add(transactionLine);
        //         }
        //     }
        // }



        // /// <summary>
        // /// Method called when the selected Filter changes
        // /// </summary>
        // partial void OnSelectedFilterChanged(string value)
        // {
        //     if (value != null)
        //     {
        //         if (value == "Quotidien")
        //         {
        //             LoadOrdersForADateAsync(SelectedDate);
        //         }
        //         else
        //         {
        //             LoadOrdersForAMonthAsync(SelectedDate.Month, SelectedDate.Year);
        //         }
        //     }
        // }

        // /// <summary>
        // /// Method called when the selected date changes
        // /// </summary>
        // partial void OnSelectedDateChanged(DateTime value)
        // {
        //     if (SelectedFilter == "Quotidien")
        //     {
        //         LoadOrdersForADateAsync(SelectedDate);
        //     }
        //     else
        //     {
        //         LoadOrdersForAMonthAsync(SelectedDate.Month, SelectedDate.Year);
        //     }
        // }


        // /// <summary>
        // /// Method to load the orders for a date
        // /// </summary>
        // private async void LoadOrdersForADateAsync(DateTime selectedDate)
        // {
        //     var ordersForDate = await DAL.OrderFactory.GetByDateAsync(selectedDate);
        //     Orders.Clear();

        //     foreach (var order in ordersForDate)
        //     {
        //         Orders.Add(order);
        //     }

        //     await CreateTransactionLines();
        // }

        // /// <summary>
        // /// Method to load order for a month and a year
        // /// </summary>
        // private async void LoadOrdersForAMonthAsync(int month, int year)
        // {
        //     var ordersForMonth = await DAL.OrderFactory.GetByMonthAndYearAsync(month, year);
        //     Orders.Clear();

        //     foreach (var order in ordersForMonth)
        //     {
        //         Orders.Add(order);
        //     }

        //     await CreateTransactionLines();
        // }

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            SelectedDate = DateTime.Now;
            FilterList = new List<string> { "Quotidien", "Mensuel" };
            SelectedFilter = "Quotidien";
            Orders = new ObservableCollection<Order>();
            TransactionLines = new ObservableCollection<TransactionLine>();
        }

        /// <summary>
        /// Create fake transactions lines
        /// </summary>
        /// <returns></returns>
        public async Task CreateTransactionLines()
        {
            TransactionLines.Clear();

            var transactionLine1 = new TransactionLine("89777", DateTime.Now, 107, "Example1@example.com", 2);
            var transactionLine2 = new TransactionLine("89778", DateTime.Now, 107, "Example2@example.com", 4);
            var transactionLine3 = new TransactionLine("89779", DateTime.Now, 107, "Example3@example.com", 1);
            var transactionLine4 = new TransactionLine("89780", DateTime.Now, 107, "Example4@example.com", 3);

            TransactionLines.Add(transactionLine1);
            TransactionLines.Add(transactionLine2);
            TransactionLines.Add(transactionLine3);
            TransactionLines.Add(transactionLine4);
        }

        #endregion
    }
}
