using FamilyAppointmentsMobile.Helpers;

namespace FamilyAppointmentsMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DispatcherHelper.Initialize();
            MainPage = new AppShell();
        }
    }
}
