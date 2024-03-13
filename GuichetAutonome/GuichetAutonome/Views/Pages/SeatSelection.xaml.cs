using GuichetAutonome.ViewModels.Pages;
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

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for SeatSelection.xaml
    /// </summary>
    public partial class SeatSelection : Page
    {
        public SeatSelection(Representation representation, int numberOfTicket)
        {
            InitializeComponent();
            VMSeatSelection vMSeatSelection = new VMSeatSelection(representation, numberOfTicket);
            this.DataContext = vMSeatSelection;
        }
    }
}
