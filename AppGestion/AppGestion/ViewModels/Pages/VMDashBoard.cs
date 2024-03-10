using AppGestion.DataAccessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMDashBoard : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The PieChart.
        /// </summary>
        [ObservableProperty]
        private SeriesCollection _pieSeries;

        /// <summary>
        /// The Line series.
        /// </summary>
        [ObservableProperty]
        private SeriesCollection _lineSeries;

        /// <summary>
        /// The grid of months for the LineChart.
        /// </summary>
        [ObservableProperty]
        private string[] _monthLabels;

        /// <summary>
        /// The current year.
        /// </summary>
        [ObservableProperty]
        private string _year;

        /// <summary>
        /// Number of events to come.
        /// </summary>
        [ObservableProperty]
        private int _eventsToCome;

        /// <summary>
        /// Number of passed events.
        /// </summary>
        [ObservableProperty]
        private int _passedEvents;

        /// <summary>
        /// Observable collection of actif clients.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<SeatSwiftDLL.Client> _actifClients;

        /// <summary>
        /// Get the number of order by month.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Tuple<string, int>> _orderByMonth;

        /// <summary>
        /// Number of show by type
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Tuple<ShowType, int>> _showByType;

        [ObservableProperty]
        private AxesCollection _yAxes;
        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMDashBoard()
        {
            Initialize();

            Task.Run(async () =>
            {
                await UpdatePageAsync();

                // Use Dispatcher to update UI elements
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LoadPieChart();
                    LoadLineChart();
                });

            }).ContinueWith((task) =>
            {
                if (task.Exception != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("An error occurred: " + task.Exception.Message);
                    });
                }
            });

            Task.Run(() => GetYear());
        }


        #endregion


        #region Commands



        #endregion


        #region Methods

        /// <summary>
        /// Load the PieChart event by type using the ShowByType
        /// </summary>
        public void LoadPieChart()
        {
            PieSeries = new SeriesCollection();

            foreach (var show in ShowByType)
            {
                PieSeries.Add(new PieSeries
                {
                    Title = show.Item1.ToString(),
                    Values = new ChartValues<ObservableValue> { new ObservableValue(show.Item2) },
                    DataLabels = true
                });
            }
        }

        /// <summary>
        /// Load the LineChart sales by month using the OrderByMonth
        /// </summary>
        public void LoadLineChart()
        {
            MonthLabels = new string[]
            {
            "Jan", "Fev", "Mar", "Avr", "Mai", "Jun",
            "Jul", "Aou", "Sep", "Oct", "Nov", "Dec"
            };

            LineSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Billets vendus :",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "01").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "02").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "03").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "04").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "05").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "06").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "07").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "08").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "09").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "10").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "11").Item2),
                        new ObservableValue(OrderByMonth.FirstOrDefault(x => x.Item1 == "12").Item2)
                    }
                }
            };
        }

        /// <summary>
        /// Get the current year
        /// </summary>
        public void GetYear()
        {
            // Get the current year
            Year = DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// Initialize properties
        /// </summary>
        public void Initialize()
        {
            PieSeries = new SeriesCollection();
            LineSeries = new SeriesCollection();
            MonthLabels = new string[] { "Jan", "Fev", "Mar", "Avr", "Mai", "Jun", "Jul", "Aou", "Sep", "Oct", "Nov", "Dec" };
            EventsToCome = 0;
            PassedEvents = 0;
            ActifClients = new ObservableCollection<SeatSwiftDLL.Client>();
            ShowByType = new ObservableCollection<Tuple<ShowType, int>>();
            OrderByMonth = new ObservableCollection<Tuple<string, int>>();
        }

        /// <summary>
        /// Update the page
        /// </summary>
        public async Task UpdatePageAsync()
        {
            try
            {
                // Fetch data asynchronously
                var shows = await DAL.ShowFactory.GetCountActiveByTypeAsync();
                var clients = await DAL.ClientFactory.GetAllActiveAsync();
                var orders = await DAL.OrderFactory.GetSellByMonthAsync();
                var upcomingEvents = await DAL.RepresentationFactory.GetCountInComingAsync();
                var pastEvents = await DAL.RepresentationFactory.GetCountPassedAsync();

                // Update the UI-bound collections on the UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Clear the lists
                    ShowByType.Clear();
                    ActifClients.Clear();
                    OrderByMonth.Clear();

                    // Add the items to the lists
                    foreach (var show in shows)
                    {
                        ShowByType.Add(show);
                    }
                    foreach (var client in clients)
                    {
                        ActifClients.Add(client);
                    }
                    foreach (var order in orders)
                    {
                        OrderByMonth.Add(order);
                    }

                    // Set the event counts
                    EventsToCome = upcomingEvents;
                    PassedEvents = pastEvents;
                });
            }
            catch (Exception ex)
            {
                // Show a message box with the error
                MessageBox.Show(ex.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        #endregion
    }
}
