

namespace FamilyAppointmentsMobile.Services
{
    public interface IShellNavigationService
    {   
        event EventHandler<ShellNavigatedEventArgs> NavigationChanged;
        event EventHandler<string> NavigationLayoutChanged;
        void OnNavigationLayoutChanged(string title);
        Task NavigateTo(string url);
        Task GoBack();
    }
}
