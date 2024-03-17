using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            VMRegistration vMRegistration = new VMRegistration();
            this.DataContext = vMRegistration;
        }

        public Registration(SeatSwiftDLL.Client client)
        {
            InitializeComponent();
            VMRegistration vMRegistration = new VMRegistration(client);
            this.DataContext = vMRegistration;
        }
    }
}
