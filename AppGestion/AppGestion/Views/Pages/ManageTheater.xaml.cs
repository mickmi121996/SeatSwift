using AppGestion.ViewModels;
using AppGestion.ViewModels.Pages;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace AppGestion.Views.Pages
{
    public partial class ManageTheater : Page
    {
        #region Constructor

        public ManageTheater()
        {
            InitializeComponent();
            VMManageTheater viewModel = new VMManageTheater();
            this.DataContext = viewModel;

            viewModel.SeatsInitialized += ViewModel_SeatsInitialized;

            this.Unloaded += ManageTheater_Unloaded;
        }

        private void ViewModel_SeatsInitialized()
        {
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
            if (viewModel == null || viewModel.SelectedAuditorium == null)
                return;

            var rows = viewModel.SelectedAuditorium.NumberOfRows;
            var columns = viewModel.SelectedAuditorium.NumberOfColumns;

            var grid = this.SeatsGrid;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                );
            }

            for (int j = 0; j < columns; j++)
            {
                grid.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                );
            }

            foreach (var seat in viewModel.Seats)
            {
                var button = new ToggleButton
                {
                    Content = seat.SeatNumber,
                    Margin = new Thickness(2),
                    IsChecked = seat.Status == SeatStatus.InService,
                    Style = (Style)FindResource("SeatToggleButtonStyle"),
                };

                if (seat.XCoordinate >= 0 && seat.YCoordinate >= 0)
                {
                    Grid.SetRow(button, seat.YCoordinate - 1);
                    Grid.SetColumn(button, seat.XCoordinate);
                    grid.Children.Add(button);
                }

                button.Checked += async (s, e) =>
                    await ((VMManageTheater)this.DataContext).ToggleSeat(seat);
                button.Unchecked += async (s, e) =>
                    await ((VMManageTheater)this.DataContext).ToggleSeat(seat);
            }

            VMMainWindow.Instance.IsCurrentlyWorking = Visibility.Collapsed;
        }

        private Brush GetColorFromSectionName(SectionName sectionName)
        {
            switch (sectionName)
            {
                case SectionName.Parterre:
                    return Brushes.Green;
                case SectionName.Balcon:
                    return Brushes.Purple;
                case SectionName.Loge:
                    return Brushes.Blue;
                default:
                    return Brushes.Gray;
            }
        }

        private void UpdateButtonColor(ToggleButton button, SectionName sectionName, bool isChecked)
        {
            if (button == null)
                return;

            button.Background = isChecked ? GetColorFromSectionName(sectionName) : Brushes.Gray;
        }

        #endregion
    }
}
