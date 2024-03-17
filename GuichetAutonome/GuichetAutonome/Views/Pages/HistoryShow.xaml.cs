using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for HistoryShow.xaml
    /// </summary>
    public partial class HistoryShow : Page
    {
        public HistoryShow()
        {
            InitializeComponent();
            VMHistoryShow vMHistoryShow = new VMHistoryShow();
            this.DataContext = vMHistoryShow;
        }
    }
}
