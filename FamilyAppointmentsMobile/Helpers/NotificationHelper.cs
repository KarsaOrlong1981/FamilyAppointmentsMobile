

using Android.App;

namespace FamilyAppointmentsMobile.Helpers
{
    public static class NotificationHelper
    {
        public static string GetDeviceToken()
        {
            if (Preferences.ContainsKey(Constants.DeviceTokenKey))
            {
                return Preferences.Get(Constants.DeviceTokenKey, "");
            }
            return string.Empty;
        }

        public static void SetDeviceToken(string token)
        {
            Preferences.Set(Constants.DeviceTokenKey, token);
        }
    }
}
