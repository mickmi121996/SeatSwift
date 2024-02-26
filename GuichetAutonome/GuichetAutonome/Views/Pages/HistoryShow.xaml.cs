using GuichetAutonome.ViewModels.Pages;
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
