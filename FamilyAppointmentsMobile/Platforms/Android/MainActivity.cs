using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
        private readonly ILogger<MainActivity> log;

        public MainActivity()
        {
            log = Ioc.Default.GetService<ILogger<MainActivity>>();
        }
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
            log.LogInformation($"App version: {info.VersionName}, version code: {info.VersionCode} started.");
            Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor(Constants.StatusBarColor));
            Window?.SetNavigationBarColor(Android.Graphics.Color.ParseColor(Constants.StatusBarColor));
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.PostNotifications }, 1);
            }
            CreateNotificationChannel();
            stopwatch.Stop();
            log.LogInformation($"OnCreate finished in {stopwatch.ElapsedMilliseconds} ms.");
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
