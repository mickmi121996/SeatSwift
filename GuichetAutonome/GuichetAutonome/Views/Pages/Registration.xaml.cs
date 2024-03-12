using GuichetAutonome.ViewModels.Pages;
using MySqlX.XDevAPI;
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
