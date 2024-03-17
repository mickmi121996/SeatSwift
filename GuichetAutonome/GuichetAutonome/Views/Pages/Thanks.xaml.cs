using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for Thanks.xaml
    /// </summary>
    public partial class Thanks : Page
    {
        public Thanks()
        {
            InitializeComponent();
            VMThanks vMThanks = new VMThanks();
            this.DataContext = vMThanks;
        }
    }
}
