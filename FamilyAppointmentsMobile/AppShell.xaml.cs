namespace FamilyAppointmentsMobile
{
    public partial class AppShell : Shell
    {
        public static AppShell? Instance;
        public AppShell()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}
