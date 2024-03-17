using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for ManageEvent.xaml
    /// </summary>
    public partial class ManageEvent : Page
    {
        public ManageEvent()
        {
            InitializeComponent();
            VMManageEvent vMManageEvent = new VMManageEvent();
            this.DataContext = vMManageEvent;
        }
    }
}
