using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGestion.ViewModels.Pages
{
    public partial class VMTransactionReport : ObservableObject
    {
        #region properties



        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMTransactionReport()
        {
            InitializeProperties();
        }

        #endregion


        #region commands

        /// <summary>
        /// Open the file exporer to save the report
        /// </summary>
        [RelayCommand]
        public void SaveReport()
        {
            // Open the file explorer
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save the report";
            saveFileDialog.ShowDialog();
        }

        #endregion


        #region methods

        /// <summary>
        /// Initialize the properties
        /// </summary>
        private void InitializeProperties()
        {
            
        }

        #endregion
    }
}
