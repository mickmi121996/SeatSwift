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
    /// Interaction logic for SeatControl.xaml
    /// </summary>
    public partial class SeatControl : UserControl
    {
        public SeatControl(Seat seat)
        {
            InitializeComponent();
            SeatViewModel seatViewModel = new SeatViewModel(seat);
            this.DataContext = seatViewModel;
        }
    }
}
