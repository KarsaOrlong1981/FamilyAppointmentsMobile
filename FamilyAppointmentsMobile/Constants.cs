

namespace FamilyAppointmentsMobile
{
    public static class Constants
    {
        public static readonly string StatusBarColor = "#393939";
        public static readonly string CalendarPage = "CalendarPage";
        public static readonly string ListPage = "ListPage";
        public static readonly string ListDetailsPage = "ListDetailsPage";
        public static readonly string MainDetailsPage = "MainDetailsPage";
        public static readonly string PendingItemsPage = "PendingItemsPage";
        public const string DatabaseFilenamePending = "PendingAppointmentsDatabase.db2";
        public const string DatabaseFilenameLocal = "LocalAppointmentsDatabase.db4";

        public static string DatabasePathPending =>
           Path.Combine(FileSystem.AppDataDirectory, DatabaseFilenamePending);
        public const SQLite.SQLiteOpenFlags FlagsPending =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePathLocal =>
           Path.Combine(FileSystem.AppDataDirectory, DatabaseFilenameLocal);
        public const SQLite.SQLiteOpenFlags FlagsLocal =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        //Firebase Pushnotifications
        public static readonly string NavigationID = "NavigationID";
        public static readonly int NotificationID = 101;
        public static readonly string DeviceTokenKey = "DeviceToken";
        public static readonly string PushChannelID = "appointments_push_channel";
        public static readonly string FireBaseAppID = "1:898356465096:android:149e61725461d2f2a384fd";
        public static readonly string FireBaseSenderID = "898356465096";
        public static readonly string FireBaseAPIkey = "AIzaSyCqGNQOX3GyUptif55YPojq0Xfp85z76T8";
        public static readonly string FireBaseProjectID = "fir-notificationsservice";

        public const string WebPushCert = "BFgsYjEwoaQ8b6v0AhCqB9u7h9GVzDRg7zu_0UWPV7EN1BAGBjBm0uXimwV4N8jWqtT6xiYJZTMv-ldBEM4N2oM";
    }
}
