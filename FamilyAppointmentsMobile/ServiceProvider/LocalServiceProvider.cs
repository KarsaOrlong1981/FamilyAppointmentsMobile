using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Services;
using FamilyAppointmentsMobile.ViewModels;
using Mopups.Interfaces;
using Mopups.Services;

namespace FamilyAppointmentsMobile.ServiceProvider
{
    public class LocalServiceProvider
    {

        public LocalServiceProvider()
        {
            InitServices();
        }

        private void InitServices()
        {
            try
            {
                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    // Services
                    .AddSingleton<IPopupNavigation>(MopupService.Instance)
                    .AddSingleton<IDialogService, DialogService>() 
                    .AddSingleton<IRestClientService, RestServiceClient>()
                    .AddSingleton<IShellNavigationService, ShellNavigationService>()
                    .AddSingleton<IConnectionService, ConnectionService>()
                    .AddSingleton<IAppointmentsTransferService, AppointmentsTransferService>()
                    // Singleton ViewModels
                    .AddSingleton<NavigationBarViewModel>()
                    .AddSingleton<MainViewModel>()
                    .BuildServiceProvider()
                    );
            } 
            catch (Exception ex) 
            {

            }
        }

        public MainViewModel MainView => Ioc.Default.GetService<MainViewModel>();
        public NavigationBarViewModel NavBar => Ioc.Default.GetService<NavigationBarViewModel>();
    }
}
