using AppGestion.ViewModels.Pages;
using System.Windows.Controls;

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for ResendEmail.xaml
    /// </summary>
    public partial class ResendEmail : Page
    {
        public ResendEmail()
        {
            InitializeComponent();
            VMResendEmail vMResendEmail = new VMResendEmail();
            this.DataContext = vMResendEmail;
        }
    }
}
