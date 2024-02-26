using GuichetAutonome.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            // Assurez-vous d'attacher le gestionnaire d'événements KeyDown
            this.KeyDown += MainWindow_KeyDown;
        }


        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Exemple : Afficher avec Ctrl + Alt + S, cacher avec Ctrl + Alt + H
            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Alt)) == (ModifierKeys.Control | ModifierKeys.Alt))
            {
                if (e.Key == Key.S) // Pour afficher
                {
                    NavigationBar.Visibility = Visibility.Visible;
                }
                else if (e.Key == Key.H) // Pour cacher
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
