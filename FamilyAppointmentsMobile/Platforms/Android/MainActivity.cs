using Android.App;
using Android.Content.PM;
using Android.OS;

namespace FamilyAppointmentsMobile
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor(Constants.StatusBarColor));
            Window?.SetNavigationBarColor(Android.Graphics.Color.ParseColor(Constants.StatusBarColor));
        }
    }
}
