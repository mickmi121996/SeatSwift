using GuichetAutonome.ViewModels.UserControls;
using SeatSwiftDLL;
using System.Windows.Controls;

namespace GuichetAutonome.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EventTemplate.xaml
    /// </summary>
    public partial class EventTemplate : UserControl
    {
        public EventTemplate(Show show)
        {
            InitializeComponent();
            VMEventTemplate vMEventTemplate = new VMEventTemplate(show);
            this.DataContext = vMEventTemplate;
        }
    }
}
