using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : Page
    {
        public Connection()
        {
            InitializeComponent();
            VMConnection vMConnection = new VMConnection();
            this.DataContext = vMConnection;
        }
    }
}
