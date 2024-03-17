using GuichetAutonome.ViewModels.Pages;
using System.Windows.Controls;

namespace GuichetAutonome.Views.Pages
{
    /// <summary>
    /// Interaction logic for EventSelection.xaml
    /// </summary>
    public partial class EventSelection : Page
    {
        public EventSelection()
        {
            InitializeComponent();
            VMEventSelection vMEventSelection = new VMEventSelection();
            this.DataContext = vMEventSelection;
        }
    }
}
