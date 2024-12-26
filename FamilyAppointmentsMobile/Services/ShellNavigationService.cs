

using FamilyAppointmentsMobile.Pages;

namespace FamilyAppointmentsMobile.Services
{
    public class ShellNavigationService : IShellNavigationService
    {
        public ShellNavigationService() 
        {
            RegisterRoutes();
            AppShell.Instance.Navigated += Instance_Navigated;
        }

        public event EventHandler<ShellNavigatedEventArgs> NavigationChanged;
        public event EventHandler<string> NavigationLayoutChanged;

        private void Instance_Navigated(object? sender, ShellNavigatedEventArgs e)
        {
            NavigationChanged?.Invoke(this, e);
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(Constants.CalendarPage, typeof(CalendarPage));
            Routing.RegisterRoute(Constants.ListPage, typeof(ListPage));
            Routing.RegisterRoute(Constants.MainDetailsPage, typeof(MainDetailsPage));
            Routing.RegisterRoute(Constants.ListDetailsPage, typeof(ListDetailPage));
            Routing.RegisterRoute(Constants.PendingItemsPage, typeof(PendingItemsPage));
           
        }
        public async Task GoBack()
        {
                await Shell.Current.GoToAsync("..");
        }

        public async Task NavigateTo(string url)
        {
            // only use this to go back to mainpage on every navigation go back step

            var stack = Shell.Current.Navigation.NavigationStack.ToArray();
            for (int i = stack.Length - 1; i > 0; i--)
            {
                Shell.Current.Navigation.RemovePage(stack[i]);
            }
            await Shell.Current.GoToAsync(url);
        }

        public void OnNavigationLayoutChanged(string title)
        {
            NavigationLayoutChanged?.Invoke(this, title);
        }
    }
}
