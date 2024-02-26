using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMSeatSelection : ObservableObject
    {
        #region Properties



        #endregion


        #region Constructor

        public VMSeatSelection()
        {
            // Constructor
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



        #endregion
    }
}
