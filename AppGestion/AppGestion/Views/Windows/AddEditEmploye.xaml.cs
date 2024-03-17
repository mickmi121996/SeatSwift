using AppGestion.ViewModels.Windows;
using SeatSwiftDLL;
using System.Windows;
using System.Windows.Input;

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

        public AddEditEmploye(User user)
        {
            InitializeComponent();
            VMAddEditEmploye vMAddEditEmploye = new VMAddEditEmploye(user);
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
