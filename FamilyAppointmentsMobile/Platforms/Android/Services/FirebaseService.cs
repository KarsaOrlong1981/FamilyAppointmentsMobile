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
            SendNotification(notification.Body, notification.Title, message.Data);
            _restClientService.OnAppointmentsChanged(null);
            _restClientService.OnTodosChanged();
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
    }
}
