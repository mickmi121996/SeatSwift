using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Page
    {
        public Checkout()
        {
            InitializeComponent();
            VMCheckout vMCheckout = new VMCheckout();
            this.DataContext = vMCheckout;
        }
    }
}
