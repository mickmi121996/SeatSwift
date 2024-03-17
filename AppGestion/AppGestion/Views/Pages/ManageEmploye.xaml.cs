using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for ManageEmploye.xaml
    /// </summary>
    public partial class ManageEmploye : Page
    {
        public ManageEmploye()
        {
            InitializeComponent();
            VMManageEmploye vMManageEmploye = new VMManageEmploye();
            this.DataContext = vMManageEmploye;
        }
    }
}
