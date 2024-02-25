using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAddEditRepresentation : ObservableObject
    {
        #region properties

        /// <summary>
        /// The current date
        /// </summary>
        [ObservableProperty]
        private DateTime _currentDate;

        /// <summary>
        /// The current time
        /// </summary>
        [ObservableProperty]
        private DateTime _currentTime;

        /// <summary>
        /// The title of the window
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// The selected date
        /// </summary>
        [ObservableProperty]
        private DateTime _selectedDate;

        /// <summary>
        /// The selected time
        /// </summary>
        [ObservableProperty]
        private DateTime _selectedTime;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMAddEditRepresentation()
        {
            InitializeProperties();
        }

        #endregion


        #region commands



        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            GetCurrentDate();
            GetCurrentTime();
            Title = "Ajouter une représentation";
            SelectedDate = CurrentDate;
            SelectedTime = CurrentTime;
        }

        /// <summary>
        /// Get the current date
        /// </summary>
        private void GetCurrentDate()
        {
            CurrentDate = DateTime.Now;
        }

        /// <summary>
        /// Get the current time
        /// </summary>
        private void GetCurrentTime()
        {
            CurrentTime = DateTime.Now;
        }

        #endregion
    }
}
