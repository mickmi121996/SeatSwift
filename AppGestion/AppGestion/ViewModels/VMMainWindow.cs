using AppGestion.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels
{
    public partial class VMMainWindow :ObservableObject
    {
        #region Instance

        /// <summary>
        /// The instance of the MainWindowVM
        /// </summary>
        public static VMMainWindow Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new VMMainWindow();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Backing field for the MainWindowVM instance
        /// </summary>
        private static VMMainWindow? _instance;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMMainWindow()
        {

        }

        #endregion


        #region Commands



        #endregion


        #region Methods

        // public void ChangeUser(User user)
        // {
        //     this._user = user;

        //     // Shows the proper panels depending on the user's role
        //     IsManagerButtonVisible = Visibility.Collapsed;
        //     IsAdminPanelVisible = Visibility.Collapsed;

        //     if (User is Manager || User is Administrator)
        //     {
        //         IsManagerButtonVisible = Visibility.Visible;
        //     }

        //     if (User is Administrator)
        //     {
        //         IsAdminPanelVisible = Visibility.Visible;
        //     }
        // }

        #endregion
    }
}
