using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMDashBoard()
        {
            Initialize();
            LoadPieChart();
            LoadLineChart();
            Task.Run(() => GetYear());

        }

        #endregion


        #region Commands



        #endregion


        #region Methods

        /// <summary>
        /// Load the PieChart event by type
        /// </summary>
        public void LoadPieChart()
        {
            _pieSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Movie",
                    Values = new ChartValues<int> { 5 }
                },
                new PieSeries
                {
                    Title = "Theater",
                    Values = new ChartValues<int> { 3 }
                },
                new PieSeries
                {
                    Title = "Humor",
                    Values = new ChartValues<int> { 2 }
                }
            };
        }

        /// <summary>
        /// Load the LineChart sales by month
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
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(8),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(5),
                        new ObservableValue(7),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(3)
                    }
                }
            };
        }

        /// <summary>
        /// Get the current year
        /// </summary>
        public async Task GetYear()
        {
            await Task.Run(() =>
            {
                // Get the current year
                Year = DateTime.Now.Year.ToString();
            });
        }

        /// <summary>
        /// Initialize properties
        /// </summary>
        public void Initialize()
        {
            PieSeries = new SeriesCollection();
            LineSeries = new SeriesCollection();
            MonthLabels = new string[] { "Jan", "Fev", "Mar", "Avr", "Mai", "Jun", "Jul", "Aou", "Sep", "Oct", "Nov", "Dec" };
        }

        #endregion
    }
}
