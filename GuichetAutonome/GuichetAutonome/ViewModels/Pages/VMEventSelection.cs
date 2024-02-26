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
    public partial class VMEventSelection : ObservableObject
    {
        #region Properties



        #endregion


        #region Constructor

        public VMEventSelection()
        {
            // Constructor
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to change page to the history page
        /// </summary>
        [RelayCommand]
        public void ChangePageToHistory()
        {
            VMMainWindow.Instance.ChangePage(typeof(HistoryShow));
        }

        /// <summary>
        /// Command to change page to the cart page
        /// </summary>
        [RelayCommand]
        public void ChangePageToCart()
        {
            VMMainWindow.Instance.ChangePage(typeof(CartShow));
        }

        #endregion


        #region Methods



        #endregion
    }
}
