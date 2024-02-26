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
    public partial class VMCartShow : ObservableObject
    {
        #region Properties



        #endregion


        #region Constructor

        public VMCartShow()
        {
            // Constructor
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the checkout page
        /// </summary>
        [RelayCommand]
        public void ChangePageToCheckout()
        {
            VMMainWindow.Instance.ChangePage(typeof(Checkout));
        }

        /// <summary>
        /// Command to the event selection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToEventSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        #endregion


        #region Methods



        #endregion
    }
}
