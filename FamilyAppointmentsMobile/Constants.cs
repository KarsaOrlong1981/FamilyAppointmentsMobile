

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
        public const string DatabaseFilenameART = "ARTeventsDatabase.db1";

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

        public static string DatabasePathARTevents =>
           Path.Combine(FileSystem.AppDataDirectory, DatabaseFilenameART);
        public const SQLite.SQLiteOpenFlags FlagsARTevents =
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

        public const string Sonstiges = "Sonstiges";
        public const string BrotUndBackwaren = "Brot und Backwaren";
        public const string ObstUndGemuese = "Obst und Gemüse";
        public const string FleischUndWurst = "Fleisch und Wurstwaren";
        public const string FischUndMeer = "Fisch und Meeresfrüchte";
        public const string Milchprodukte = "Milchprodukte";
        public const string Tiefkuehlkost = "Tiefkühlkost";
        public const string Getraenke = "Getränke";
        public const string TrockenProdukte = "Trockenprodukte";
        public const string Snacks = "Snacks und Süßigkeiten";
        public const string Gewuerze = "Gewürze und Saucen";
        public const string Konserven = "Konserven und Fertiggerichte";
        public const string Drogerie = "Drogerie";
        public const string Spirituosen = "Spirituosen";
        public const string Haushaltwaren = "Haushaltwaren";
        public const string Tierbedarf = "Tierbedarf";
        public const string Schreibwaren = "Schreibwaren";
        public const string Elektronik = "Elektronik";
        public const string Kleidung = "Kleidung und Schuhe";
        public const string Spielwaren = "Spielwaren";
        public const string BuecherZeitschriften = "Bücher und Zeitschriften";
        public const string BlumenPflanzen = "Blumen und Pflanzen";
        public const string SportFreizeit = "Sport und Freizeit";
    }
}
