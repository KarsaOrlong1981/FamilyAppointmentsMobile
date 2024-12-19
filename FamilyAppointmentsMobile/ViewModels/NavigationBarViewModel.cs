using FamilyAppointmentsMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class NavigationBarViewModel : ObservableObject
    {
        [ObservableProperty] private string title;

        [ObservableProperty]
        private bool canGoBack = true;
        [ObservableProperty]
        private bool isConnected;

        private IShellNavigationService shellNavigationService;
        private IConnectionService connectionService;
        private IAppointmentsTransferService appointmentsTransferService;
        public NavigationBarViewModel() 
        {
            shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            shellNavigationService.NavigationChanged += ShellNavigationService_NavigationChanged;
            shellNavigationService.NavigationLayoutChanged += ShellNavigationService_NavigationLayoutChanged;
            connectionService.ConnectionChanged += ConnectionService_ConnectionChanged;
        }

        private void ShellNavigationService_NavigationLayoutChanged(object? sender, string e)
        {
            Title = e;
        }

        private void ConnectionService_ConnectionChanged(object? sender, bool e)
        {
            IsConnected = e;
        }

        private void ShellNavigationService_NavigationChanged(object? sender, ShellNavigatedEventArgs e)
        {
           var tempTitle = GetPageTitle(e.Current.Location.OriginalString);
           Title = tempTitle;
        }

        [RelayCommand]
        private async Task ConnectLocal()
        {        
            IsConnected = await connectionService.LocalConnection();
        }

        private string GetPageTitle(string locationUri)
        {
            CanGoBack = true;
            var result = string.Empty;
            var location = locationUri.Split('/').Last();
            if (location == "MainPage")
            {
                result = "Familien Mitglieder";
                CanGoBack = false;
            }
            else if (location == Constants.CalendarPage)
            {
                result = "Kalender";
            }
            else if (location == Constants.TodayPage)
            {
                result = "Heute";
            }
            else if (location == Constants.PendingItemsPage)
            {
                result = "In der Warteschlange";
            }
            return result;
        }

        [RelayCommand]
        private async Task GoBack()
        {
            await shellNavigationService.GoBack();
        }

        [RelayCommand]
        private async Task Refresh()
        {
            try
            {
                await appointmentsTransferService.LoadAppointments();
            }
            catch (Exception ex) { }
        }
    }
}
