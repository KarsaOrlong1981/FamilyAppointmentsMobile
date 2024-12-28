using SQLite;

namespace FamilyAppointmentsMobile.Models
{
    public class ARTEvents
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public int Year { get; set; }
    }
}
