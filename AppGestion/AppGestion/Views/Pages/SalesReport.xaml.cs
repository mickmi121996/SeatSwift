using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class SalesReport : Page
    {
        public SalesReport()
        {
            InitializeComponent();
            VMSalesReport vMSalesReport = new VMSalesReport();
            this.DataContext = vMSalesReport;
        }
    }
}
