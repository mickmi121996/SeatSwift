using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for CartShow.xaml
    /// </summary>
    public partial class CartShow : Page
    {
        public CartShow()
        {
            InitializeComponent();
            VMCartShow vMCartShow = new VMCartShow();
            this.DataContext = vMCartShow;
        }
    }
}
