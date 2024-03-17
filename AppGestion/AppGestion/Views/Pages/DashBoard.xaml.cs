using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        public DashBoard()
        {
            InitializeComponent();
            VMDashBoard VM = new VMDashBoard();
            this.DataContext = VM;
        }
    }
}
