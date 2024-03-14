using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views.Pages;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMSeatSelection : ObservableObject
    {
        #region Properties

        /// <summary>
        /// The visibility of the Selection filter
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedFilter))]
        private Visibility _isSelectionFilterVisible;

        /// <summary>
        /// The observable collection of SectionName
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _sectionNames;

        /// <summary>
        /// The selected section name
        /// </summary>
        [ObservableProperty]
        private string _selectedSectionName;

        /// <summary>
        /// The observable collection of Filter
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _filters;

        /// <summary>
        /// The selected filter
        /// </summary>
        [ObservableProperty]
        private string _selectedFilter;

        /// <summary>
        /// The observable collection of tickets for the representation
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Ticket> _tickets;

        /// <summary>
        /// The representation
        /// </summary>
        [ObservableProperty]
        private Representation _representation;

        /// <summary>
        /// The number of tickets
        /// </summary>
        [ObservableProperty]
        private int _numberOfTickets;

        #endregion


        #region Constructor

        public VMSeatSelection(Representation representation, int numberOfTicket)
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            _representation = representation;
            _numberOfTickets = numberOfTicket;

            InitializeFilterAndSectionName();
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
        /// Command to add the order to the cart
        /// </summary>
        [RelayCommand]
        public void AddOrderToCart()
        {
            // Add the order to the cart
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize The list of filter and section name
        /// </summary>
        private void InitializeFilterAndSectionName()
        {
            _sectionNames = new ObservableCollection<string>();
            _filters = new ObservableCollection<string>();

            // Get the list of section name in the enumeration
            foreach (var section in Enum.GetValues(typeof(SectionName)))
            {
                SectionNames.Add(section.ToString());
            }

            //Select the first section
            SelectedSectionName = SectionNames[0];



            // Get the list of filter with Salle and Section
            Filters.Add("Salle");
            Filters.Add("Section");

            // Select the first filter
            SelectedFilter = Filters[0];
        }


        /// <summary>
        /// Method called when the selected Auditorium changes
        /// </summary>
        partial void OnSelectedFilterChanged(string value)
        {
            if (value != null)
            {
                // Check if the selected filter is Salle
                if (value == "Salle")
                {
                    IsSelectionFilterVisible = Visibility.Collapsed;
                }
                else
                {
                    IsSelectionFilterVisible = Visibility.Visible;
                }
            }
        }

        #endregion
    }
}
