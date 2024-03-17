using AppGestion.ViewModels;
using AppGestion.Views.Windows;
using SeatSwiftDLL;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AppGestion.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User user)
        {
            InitializeComponent();
            VMMainWindow.Instance.ChangeUser(user);
            VMMainWindow vm = VMMainWindow.Instance;
            DataContext = vm;
            fContainer.Navigate(
                new System.Uri("Views/Pages/Home.xaml", UriKind.RelativeOrAbsolute)
            );
            vm.Title = "Accueil";
        }

        #region Left menu Button

        /// <summary>
        /// Show the popup Home when the mouse enter the button
        /// </summary>
        private void btnHome_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnHome;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Accueil";
            }
        }

        /// <summary>
        /// Hide the popup Home when the mouse leave the button
        /// </summary>
        private void btnHome_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Dashboard when the mouse enter the button
        /// </summary>
        private void btnDashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnDashboard;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Tableau de bord";
            }
        }

        /// <summary>
        /// Hide the popup Dashboard when the mouse leave the button
        /// </summary>
        private void btnDashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Manage Event when the mouse enter the button
        /// </summary>
        private void btnManageEvent_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnManageEvent;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Gestion des événements";
            }
        }

        /// <summary>
        /// Hide the popup Manage Event when the mouse leave the button
        /// </summary>
        private void btnManageEvent_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Manage Emp when the mouse enter the button
        /// </summary>
        private void btnManageEmp_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnManageEmp;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Gestion des employés";
            }
        }

        /// <summary>
        /// Hide the popup Manage Emp when the mouse leave the button
        /// </summary>
        private void btnManageEmp_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Manage Theater when the mouse enter the button
        /// </summary>
        private void btnManageTheater_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnManageTheater;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Gestion des salles";
            }
        }

        /// <summary>
        /// Hide the popup Manage Theater when the mouse leave the button
        /// </summary>
        private void btnManageTheater_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Sales Report when the mouse enter the button
        /// </summary>
        private void btnSalesReport_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnSalesReport;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Rapport de vente";
            }
        }

        /// <summary>
        /// Hide the popup Sales Report when the mouse leave the button
        /// </summary>
        private void btnSalesReport_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup Transaction Report when the mouse enter the button
        /// </summary>
        private void btnTransactionReport_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnTransactionReport;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Rapport de transaction";
            }
        }

        /// <summary>
        /// Hide the popup Transaction Report when the mouse leave the button
        /// </summary>
        private void btnTransactionReport_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup About when the mouse enter the button
        /// </summary>
        private void btnAbout_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnAbout;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "A propos";
            }
        }

        /// <summary>
        /// Hide the popup About when the mouse leave the button
        /// </summary>
        private void btnAbout_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        /// <summary>
        /// Show the popup About when the mouse enter the button
        /// </summary>
        private void btnResend_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnResend;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Renvoyer billets";
            }
        }

        /// <summary>
        /// Hide the popup About when the mouse leave the button
        /// </summary>
        private void btnResend_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }
        #endregion


        #region Button Close | Restore | Minimize

        /// <summary>
        /// Close the application
        /// </summary>
        private void AlertColor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Restore the application
        /// </summary>
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Minimize the application
        /// </summary>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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


        #region Navigation

        /// <summary>
        /// Navigate to the Home page
        /// </summary>
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/Home.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Accueil";
        }

        /// <summary>
        /// Navigate to the Dashboard page
        /// </summary>
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/DashBoard.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Tableau de bord";
        }

        /// <summary>
        /// Navigate to the Manage Event page
        /// </summary>
        private void btnManageEvent_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/ManageEvent.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Gestion des événements";
        }

        /// <summary>
        /// Navigate to the Manage Employee page
        /// </summary>
        private void btnManageEmp_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/ManageEmploye.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Gestion des employés";
        }

        /// <summary>
        /// Navigate to the Manage Theater page
        /// </summary>
        private void btnManageTheater_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/ManageTheater.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Configuration des salles";
        }

        /// <summary>
        /// Navigate to the Sales Report page
        /// </summary>
        private void btnSalesReport_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/SalesReport.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Rapport de vente";
        }

        /// <summary>
        /// Navigate to the Transaction Report page
        /// </summary>
        private void btnTransactionReport_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/TransactionReport.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Rapport de transaction";
        }

        /// <summary>
        /// Navigate to the Transaction Report page
        /// </summary>
        private void btnResend_Click(object sender, RoutedEventArgs e)
        {
            fContainer.Navigate(
                new System.Uri("Views/Pages/ResendEmail.xaml", UriKind.RelativeOrAbsolute)
            );
            VMMainWindow.Instance.Title = "Renvoyer billet";
        }

        /// <summary>
        /// Open the about window
        /// </summary>
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        #endregion
    }
}
