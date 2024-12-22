using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

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
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.PostNotifications }, 1);
            }
            CreateNotificationChannel();
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            if (intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    if (key == Constants.NavigationID)
                    {
                        string idValue = intent.Extras.GetString(key);
                        if (Preferences.ContainsKey(Constants.NavigationID))
                            Preferences.Remove(Constants.NavigationID);

                        Preferences.Set(Constants.NavigationID, idValue);
                    }
                }
            }
        }

        private void CreateNotificationChannel()
        {
            //if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
            //{
                var channel = new NotificationChannel(Constants.PushChannelID, "Appointments Notification Channel", NotificationImportance.Default);
                var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            //}
        }

    }
}
