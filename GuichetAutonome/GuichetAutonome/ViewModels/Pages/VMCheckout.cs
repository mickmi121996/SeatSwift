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
    public partial class VMCheckout : ObservableObject
    {
        #region Properties



        #endregion


        #region Constructor

        public VMCheckout()
        {
            // Constructor
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the thanks page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToThanks()
        {
            VMMainWindow.Instance.ChangePage(typeof(Thanks));
        }

        /// <summary>
        /// Command to the cart page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToCart()
        {
            VMMainWindow.Instance.ChangePage(typeof(CartShow));
        }

        #endregion


        #region Methods



        #endregion
    }
}
