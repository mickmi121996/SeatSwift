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
    public partial class VMThanks : ObservableObject
    {      
        #region Properties



        #endregion


        #region Constructor

        public VMThanks()
        {
            // Constructor
        }

        #endregion


        #region Commands

        /// <summary>
        /// Command to the connection page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToConnection()
        {
            VMMainWindow.Instance.ChangePage(typeof(Connection));
        }

        /// <summary>
        /// Command to the event selection page
        /// </summary>
        [RelayCommand]
        public async Task ChangePageToEventSelection()
        {
            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }

        #endregion


        #region Methods



        #endregion
    }
}
