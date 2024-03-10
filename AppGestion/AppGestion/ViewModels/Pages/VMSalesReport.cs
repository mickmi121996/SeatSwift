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
    public partial class VMSalesReport : ObservableObject
    {
        #region properties

        /// <summary>
        /// The selected date
        /// </summary>
        [ObservableProperty]
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
        private ObservableCollection<SellLine> _sellLines;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMSalesReport()
        {
            InitializeProperties();
            CreateFakeSellLines();
        }

        #endregion


        #region commands

        /// <summary>
        /// Open the file exporer to save the report
        /// </summary>
        [RelayCommand (CanExecute = nameof(CanExecuteExport))]
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
                PDFTools.CreateSalesReportPdf(saveFileDialog.FileName, SellLines.ToList(), SelectedDate, SelectedFilter);
            }
        }

        /// <summary>
        /// Method to check if the command can be executed
        /// </summary>
        private bool CanExecuteExport()
        {
            // if there is at least one sell line
            if (SellLines.Count > 0)
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
        // /// Method to create the sell lines for the selected orders
        // /// </summary>
        // public async Task CreateSellLines()
        // {
        //     SellLines.Clear();

        //     // Dictionnaire pour regrouper les tickets par représentation
        //     var representationGroups = new Dictionary<int, List<Ticket>>();

        //     foreach (var order in Orders)
        //     {
        //         var tickets = await DAL.TicketFactory.GetByOrderAsync(order);

        //         foreach (var ticket in tickets)
        //         {
        //             // Vérifier si le groupe pour cette représentation existe déjà
        //             if(ticket.Representation == null) { throw new ArgumentNullException(nameof(ticket.Representation)); }
        //             if (!representationGroups.ContainsKey(ticket.Representation.Id))
        //             {
        //                 representationGroups[ticket.Representation.Id] = new List<Ticket>();
        //             }

        //             representationGroups[ticket.Representation.Id].Add(ticket);
        //         }
        //     }

        //     foreach (var groupId in representationGroups.Keys)
        //     {
        //         var ticketsForRepresentation = representationGroups[groupId];
        //         if (ticketsForRepresentation.Count > 0)
        //         {
        //             // Prendre la première ticket pour obtenir les informations de base
        //             var firstTicket = ticketsForRepresentation.First();
        //             if(firstTicket.Representation == null) { throw new ArgumentNullException(nameof(firstTicket.Representation)); }
        //             var representationDate = firstTicket.Representation.Date;
        //             if(firstTicket.Representation.Show == null) { throw new ArgumentNullException(nameof(firstTicket.Representation.Show)); }
        //             var showName = firstTicket.Representation.Show.Name;
        //             var ticketsSold = ticketsForRepresentation.Count;
        //             double totalAmountBeforeTaxe = ticketsForRepresentation.Sum(ticket => (double)ticket.Representation.Show.BasePrice);

        //             SellLines.Add(new SellLine(ticketsSold, representationDate, showName, totalAmountBeforeTaxe));
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

        //     await CreateSellLines();
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

        //     await CreateSellLines();
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
            SellLines = new ObservableCollection<SellLine>();
        }


        /// <summary>
        /// Create a fake list of sell lines to test the pdf creation
        /// </summary>
        private void CreateFakeSellLines()
        {
            SellLines.Add(new SellLine(10, new DateTime(2021, 10, 10), "Show 1", 10));
            SellLines.Add(new SellLine(20, new DateTime(2021, 10, 10), "Show 2", 20));
            SellLines.Add(new SellLine(30, new DateTime(2021, 10, 10), "Show 3", 30));
            SellLines.Add(new SellLine(40, new DateTime(2021, 10, 10), "Show 4", 40));
            SellLines.Add(new SellLine(50, new DateTime(2021, 10, 10), "Show 5", 50));
        }

        #endregion
    }
}
