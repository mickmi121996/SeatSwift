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
            VMMainWindow.Instance.ResetInactivityTimer();

            // Démarre un délai dès l'initialisation du ViewModel
            Task.Delay(20000).ContinueWith(_ => ChangePageToConnection(), TaskScheduler.FromCurrentSynchronizationContext());
        }


        #endregion


        #region Commands

        /// <summary>
        /// Command to the connection page
        /// </summary>
        [RelayCommand]
        public void ChangePageToConnection()
        {
            // Déconnecte l'utilisateur
            VMMainWindow.Instance.LogoutUser();
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
