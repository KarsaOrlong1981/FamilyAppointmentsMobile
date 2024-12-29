using Android.App;
using Android.Content;
using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using Firebase.Messaging;
using Microsoft.Extensions.Logging;

namespace FamilyAppointmentsMobile.Platforms.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        private readonly ILogger<FirebaseService> log;  
        private IRestClientService _restClientService;
        private HashSet<int> scheduledNotifications = new HashSet<int>();

        public FirebaseService() 
        {
            log = Ioc.Default.GetService<ILogger<FirebaseService>>();
            _restClientService = Ioc.Default.GetService<IRestClientService>();
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            if (Preferences.ContainsKey(Constants.DeviceTokenKey))
            {
                Preferences.Remove(Constants.DeviceTokenKey);
            }
            Preferences.Set(Constants.DeviceTokenKey, token);
            var newClient = new Clients { DeviceToken = token, Id = Guid.NewGuid().ToString()};
            try
            {
                _restClientService.RegisterOrUpdateClient(newClient);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to register new client or updating exsiting.");
            }
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            var notification = message.GetNotification();
            if (notification != null)
            {
                SendNotification(notification.Body, notification.Title, message.Data);
                if (notification.Title.Contains("wurde erstellt.") || notification.Title.Contains("wurde aktualisiert.") || notification.Title.Contains("wurde gelöscht."))
                {
                    _restClientService.OnTodosChangedNotificationReceived();
                }
            }
        }

        private void SendNotification(string messageBody, string title, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            foreach (var key in data.Keys)
            {
                string value = data[key];
                intent.PutExtra(key, value);
            }

            var pendingIntent = PendingIntent.GetActivity(this, Constants.NotificationID, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);

            var notificationBuilder = new AndroidX.Core.App.NotificationCompat.Builder(this, Constants.PushChannelID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentText(messageBody)
                .SetChannelId(Constants.PushChannelID)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetPriority((int)NotificationPriority.Max);

            var notificationManager = AndroidX.Core.App.NotificationManagerCompat.From(this);
            notificationManager.Notify(Constants.NotificationID, notificationBuilder.Build());
        }

        public void ScheduleNotification(ARTEvents artEvent)
        {
            // Berechne den Zeitpunkt einen Tag vorher um 16:00 Uhr
            var notificationTime = artEvent.StartTime.AddDays(-1).Date.AddHours(16);

            if (notificationTime > DateTime.Now) // Stelle sicher, dass die Benachrichtigung in der Zukunft liegt
            {
                int notificationId = artEvent.StartTime.GetHashCode();

                // Überprüfe, ob die Benachrichtigung bereits geplant wurde
                if (scheduledNotifications.Contains(notificationId))
                {
                    log.LogInformation($"Benachrichtigung für {artEvent.EventName} ist bereits geplant.");
                    return;
                }

                scheduledNotifications.Add(notificationId);

                var intent = new Intent(Android.App.Application.Context, typeof(NotificationReceiver));
                intent.PutExtra("eventName", artEvent.EventName);
                intent.PutExtra("eventTime", artEvent.StartTime.ToString());

                var pendingIntent = PendingIntent.GetBroadcast(
                    Android.App.Application.Context,
                    notificationId,
                    intent,
                    PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

                var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
                var triggerTime = (long)(notificationTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;

                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerTime, pendingIntent);

                log.LogInformation($"Benachrichtigung für {artEvent.EventName} geplant.");
            }
        }
    }
}
