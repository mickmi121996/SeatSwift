using AppGestion.ViewModels.Windows;
using SeatSwiftDLL;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AppGestion.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddEditEvent.xaml
    /// </summary>
    public partial class AddEditEvent : Window
    {
        public AddEditEvent()
        {
            InitializeComponent();
            VMAddEditEvent vMAddEditEvent = new VMAddEditEvent();
            this.DataContext = vMAddEditEvent;
        }

        public AddEditEvent(Show show)
        {
            InitializeComponent();
            VMAddEditEvent vMAddEditEvent = new VMAddEditEvent(show);
            this.DataContext = vMAddEditEvent;
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

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.E)
            {
                e.Handled = true;
            }
        }
    }
}
