using Android.App;
using Android.Content;

namespace FamilyAppointmentsMobile.Platforms.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var eventName = intent.GetStringExtra("eventName");
            var eventTime = intent.GetStringExtra("eventTime");

            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var notificationBuilder = new AndroidX.Core.App.NotificationCompat.Builder(context, Constants.PushChannelID)
                .SetContentTitle("Erinnerung A.R.T.: " + eventName)
                .SetContentText($"Morgen am {eventTime}.")
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetPriority((int)NotificationPriority.High);

            var notificationManager = AndroidX.Core.App.NotificationManagerCompat.From(context);
            notificationManager.Notify(eventName.GetHashCode(), notificationBuilder.Build());
        }
    }
}
