using FamilyAppointmentsMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Models;
using Microsoft.Extensions.Logging;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class NavigationBarViewModel : ObservableObject
    {
        private readonly ILogger<NavigationBarViewModel> log;

        [ObservableProperty] private string title;
        [ObservableProperty]
        private bool canGoBack = true;
        [ObservableProperty]
        private bool isConnected;
        [ObservableProperty] private EConnectionType connectionType;

        private IShellNavigationService shellNavigationService;
        private IConnectionService connectionService;
        private IRestClientService restClientService;
        private IAppointmentsTransferService appointmentsTransferService;
        public NavigationBarViewModel() 
        {
            log = Ioc.Default.GetService<ILogger<NavigationBarViewModel>>();
            shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            restClientService = Ioc.Default.GetService<IRestClientService>();
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            shellNavigationService.NavigationChanged += ShellNavigationService_NavigationChanged;
            shellNavigationService.NavigationLayoutChanged += ShellNavigationService_NavigationLayoutChanged;
            connectionService.ConnectionChanged += ConnectionService_ConnectionChanged;
        }

        private void ShellNavigationService_NavigationLayoutChanged(object? sender, string e)
        {
            Title = e;
        }

        private void ConnectionService_ConnectionChanged(object? sender, EConnectionType e)
        {
            IsConnected = e != EConnectionType.NotConnected;
            ConnectionType = e;
        }

        private void ShellNavigationService_NavigationChanged(object? sender, ShellNavigatedEventArgs e)
        {
           var tempTitle = GetPageTitle(e.Current.Location.OriginalString);
           Title = tempTitle;
        }

        [RelayCommand]
        private async Task ConnectToCloud()
        { 
            if (!IsConnected)
            {
               var success = await connectionService.CloudConnection();
               if (success) 
                    ConnectionType = EConnectionType.Cloud;
               else 
                    ConnectionType = EConnectionType.NotConnected;
            }
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
            else if (location == Constants.ListPage)
            {
                result = "Listen und Aufgaben";
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
                await appointmentsTransferService.LoadTodos();
            }
            catch (Exception ex) 
            {
                log.LogError(ex, "Error on refreshing lists from cloud.");
            }
        }
    }
}
