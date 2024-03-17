using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuichetAutonome.Views.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace GuichetAutonome.ViewModels.Pages
{
    public partial class VMThanks : ObservableObject
    {
        private CancellationTokenSource delayCancellation;

        public VMThanks()
        {
            VMMainWindow.Instance.ResetInactivityTimer();

            delayCancellation = new CancellationTokenSource();

            Task.Delay(20000, delayCancellation.Token)
                .ContinueWith(
                    _ =>
                    {
                        if (!_.IsCanceled)
                        {
                            ChangePageToConnection();
                        }
                    },
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }

        [RelayCommand]
        public void ChangePageToConnection()
        {
            VMMainWindow.Instance.LogoutUser();
        }

        [RelayCommand]
        public async Task ChangePageToEventSelection()
        {
            delayCancellation.Cancel();

            VMMainWindow.Instance.ChangePage(typeof(EventSelection));
        }
    }
}
