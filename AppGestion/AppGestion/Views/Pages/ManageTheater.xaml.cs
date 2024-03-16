using AppGestion.ViewModels;
using AppGestion.ViewModels.Pages;
using LiveCharts.Wpf.Charts.Base;
using Microsoft.Win32;
using Newtonsoft.Json;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppGestion.Views.Pages
{
    public partial class ManageTheater : Page
    {

        #region properties



        #endregion

        #region Constructor

        public ManageTheater()
        {
            InitializeComponent();
            VMManageTheater viewModel = new VMManageTheater();
            this.DataContext = viewModel;

            // S'abonner à l'événement SeatsInitialized
            viewModel.SeatsInitialized += ViewModel_SeatsInitialized;

            // Attacher le gestionnaire d'événements Unloaded
            this.Unloaded += ManageTheater_Unloaded;
        }


        private void ViewModel_SeatsInitialized()
        {
            // Appeler ConfigureGrid sur le thread UI
            Dispatcher.Invoke(() =>
            {
                ConfigureGrid();
            });
        }

        private void ManageTheater_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is VMManageTheater viewModel)
            {
                viewModel.SeatsInitialized -= ViewModel_SeatsInitialized;
            }

            this.Unloaded -= ManageTheater_Unloaded;
        }




        public void ConfigureGrid()
        {
            var viewModel = DataContext as VMManageTheater;
            if (viewModel == null || viewModel.SelectedAuditorium == null) return;

            var rows = viewModel.SelectedAuditorium.NumberOfRows;
            var columns = viewModel.SelectedAuditorium.NumberOfColumns;

            // Accès direct au Grid défini dans le XAML
            var grid = this.SeatsGrid;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // À l'intérieur de votre boucle foreach où vous créez les ToggleButtons
            foreach (var seat in viewModel.Seats)
            {
                var button = new ToggleButton
                {
                    Content = seat.SeatNumber,
                    Margin = new Thickness(2),
                    IsChecked = seat.Status == SeatStatus.InService,
                    Style = (Style)FindResource("SeatToggleButtonStyle"),
                };


                // Positionnez le bouton sur le Grid
                if (seat.XCoordinate >= 0 && seat.YCoordinate >= 0)
                {
                    Grid.SetRow(button, seat.YCoordinate);
                    Grid.SetColumn(button, seat.XCoordinate);
                    grid.Children.Add(button);
                }

                // Attacher les gestionnaires d'événements Checked et Unchecked
                button.Checked += async (s, e) => await ((VMManageTheater)this.DataContext).ToggleSeat(seat);
                button.Unchecked += async (s, e) => await ((VMManageTheater)this.DataContext).ToggleSeat(seat);
            }

            VMMainWindow.Instance.IsCurrentlyWorking = Visibility.Collapsed;
        }

        private Brush GetColorFromSectionName(SectionName sectionName)
        {
            switch (sectionName)
            {
                case SectionName.Parterre: return Brushes.Green;
                case SectionName.Balcon: return Brushes.Purple;
                case SectionName.Loge: return Brushes.Blue;
                default: return Brushes.Gray;
            }
        }

        // Méthode pour mettre à jour la couleur d'un bouton lors du changement d'état
        private void UpdateButtonColor(ToggleButton button, SectionName sectionName, bool isChecked)
        {
            if (button == null) return;

            button.Background = isChecked ? GetColorFromSectionName(sectionName) : Brushes.Gray;
        }



        #endregion
    }
}
