using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for TransactionReport.xaml
    /// </summary>
    public partial class TransactionReport : Page
    {
        public TransactionReport()
        {
            InitializeComponent();
            VMTransactionReport vMTransactionReport = new VMTransactionReport();
            this.DataContext = vMTransactionReport;
        }
    }
}
