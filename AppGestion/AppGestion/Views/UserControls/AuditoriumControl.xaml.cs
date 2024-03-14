using AppGestion.ViewModels;
using SeatSwiftDLL;
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

namespace AppGestion.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AuditoriumControl.xaml
    /// </summary>
    public partial class AuditoriumControl : UserControl
    {
        public AuditoriumControl()
        {
            InitializeComponent();
            Loaded += AuditoriumControl_Loaded;
        }

        private void AuditoriumControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is AuditoriumViewModel auditoriumViewModel)
            {
                InitializeSeatsGrid(auditoriumViewModel);
            }
        }

        private void InitializeSeatsGrid(AuditoriumViewModel auditoriumViewModel)
        {
            // Assurez-vous que le Grid est vide
            SeatsGrid.Children.Clear();
            SeatsGrid.RowDefinitions.Clear();
            SeatsGrid.ColumnDefinitions.Clear();

            // Ajouter des lignes et des colonnes au Grid
            for (int row = 0; row < auditoriumViewModel.Auditorium.NumberOfRows; row++)
            {
                SeatsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int col = 0; col < auditoriumViewModel.Auditorium.NumberOfColumns; col++)
            {
                SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Ajouter les sièges
            for (int y = 0; y < auditoriumViewModel.SeatsGrid.GetLength(0); y++)
            {
                for (int x = 0; x < auditoriumViewModel.SeatsGrid.GetLength(1); x++)
                {
                    var seatViewModel = auditoriumViewModel.SeatsGrid[y, x];
                    if (seatViewModel != null)
                    {
                        var seatControl = new SeatControl(seatViewModel.Seat);
                        seatControl.DataContext = seatViewModel;
                        Grid.SetRow(seatControl, y);
                        Grid.SetColumn(seatControl, x);
                        SeatsGrid.Children.Add(seatControl);
                    }
                }
            }
        }
    }

}
