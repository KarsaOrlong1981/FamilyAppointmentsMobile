

namespace FamilyAppointmentsMobile
{
    public static class Constants
    {
        public static readonly string StatusBarColor = "#393939";
        public static readonly string CalendarPage = "CalendarPage";
        public static readonly string TodayPage = "TodayPage";
        public static readonly string TodayDetailsPage = "TodayDetailsPage";
        public static readonly string MainDetailsPage = "MainDetailsPage";
        public static readonly string PendingItemsPage = "PendingItemsPage";
        public const string DatabaseFilenamePending = "PendingAppointmentsDatabase.db1";
        public const string DatabaseFilenameLocal = "LocalAppointmentsDatabase.db3";

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
    }
}
