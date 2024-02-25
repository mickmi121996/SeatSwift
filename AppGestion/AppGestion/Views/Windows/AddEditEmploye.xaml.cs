using AppGestion.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppGestion.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddEditEmploye.xaml
    /// </summary>
    public partial class AddEditEmploye : Window
    {
        public AddEditEmploye()
        {
            InitializeComponent();
            VMAddEditEmploye vMAddEditEmploye = new VMAddEditEmploye();
            this.DataContext = vMAddEditEmploye;
        }

        #region Button Close | Restore | Minimize 

        /// <summary>
        /// Close the application
        /// </summary>
        private void AlertColor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Drag the window
        /// </summary>
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion
    }
}
