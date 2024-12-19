using SQLite;

namespace FamilyAppointmentsMobile.Models
{
    public class LocalAppointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string AppointmentId { get; set; }
        public string Member { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
