using GuichetAutonome.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace GuichetAutonome.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VMMainWindow vMainWindow = VMMainWindow.Instance;
            this.DataContext = vMainWindow;

            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (
                (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Alt))
                == (ModifierKeys.Control | ModifierKeys.Alt)
            )
            {
                if (e.Key == Key.S)
                {
                    NavigationBar.Visibility = Visibility.Visible;
                }
                else if (e.Key == Key.H)
                {
                    NavigationBar.Visibility = Visibility.Collapsed;
                }
            }
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
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion
    }
}
