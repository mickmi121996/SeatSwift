using GuichetAutonome.ViewModels.UserControls;
using SeatSwiftDLL;
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

namespace GuichetAutonome.Views.UserControls
{
    /// <summary>
    /// Interaction logic for OrderTemplate.xaml
    /// </summary>
    public partial class OrderTemplate : UserControl
    {
        public OrderTemplate(List<Ticket> tickets)
        {
            InitializeComponent();
            VMOrderTemplate vMOrderTemplate = new VMOrderTemplate(tickets);
            this.DataContext = vMOrderTemplate;
        }

        public async Task InitializeAsync(List<Ticket> tickets)
        {
            // Supposons que DataContext est de type VMOrderTemplate
            if (this.DataContext is VMOrderTemplate viewModel)
            {
                await viewModel.InitializeTicketsAsync(tickets);
            }
        }
    }
}
