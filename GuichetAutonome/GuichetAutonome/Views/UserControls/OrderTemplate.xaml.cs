using GuichetAutonome.ViewModels.UserControls;
using SeatSwiftDLL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            if (this.DataContext is VMOrderTemplate viewModel)
            {
                await viewModel.InitializeTicketsAsync(tickets);
            }
        }
    }
}
