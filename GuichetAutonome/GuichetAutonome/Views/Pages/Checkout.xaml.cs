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
