using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Services;
using FamilyAppointmentsMobile.ViewModels;
using Mopups.Interfaces;
using Mopups.Services;
using Serilog;

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
                    // Logging 
#if ANDROID
                    .AddLogging(loggingBuilder =>
                    loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .WriteTo.File(
                        Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, "logs", "appointments.log"),
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                    .CreateLogger(), dispose: true))
#elif IOS

                    .AddLogging(loggingBuilder =>
                        loggingBuilder.AddSerilog(new LoggerConfiguration()
                            .WriteTo.File(
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs", "app.log"),
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                rollingInterval: RollingInterval.Day,
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{SourceContext}] [{Level}] {Message}{NewLine}{Exception}")
                            .CreateLogger(), dispose: true))
#endif
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
