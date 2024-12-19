using FamilyAppointmentsMobile.Helpers;
using SQLite;
using System.Text.Json.Serialization;

namespace FamilyAppointmentsMobile.Models
{
    public class PendingAppointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string AppointmentId { get; set; }
        public string Member { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }

        public EPendingOperationType PendingOperationType { get; set; }

        [Ignore]
        public Color Color => SetMemberColor();

        [Ignore]
        public string AppointmentDay
        {
            get
            {
                return Date.HasValue
                    ? Date.Value.ToString("dddd", new System.Globalization.CultureInfo("de-DE"))
                    : "K.A.";
            }
        }

        [Ignore]
        public string FormattedDate
        {
            get
            {
                return Date.HasValue ? $"{Date.Value:dd.MM.yyyy HH:mm}" : "K.A.";
            }
        }

        [Ignore]
        public string OperationTypeFormat => SetOperationType();

        private Color SetMemberColor()
        {
            var color = Colors.SteelBlue;
            switch(Member)
            {
                case "Joerg": color = ResourceHelper.GetResource<Color>("JoergColor"); break;
                case "Karin": color = ResourceHelper.GetResource<Color>("KarinColor"); break;
                case "Marvin": color = ResourceHelper.GetResource<Color>("MarvinColor"); break;
                case "Lio": color = ResourceHelper.GetResource<Color>("LioColor"); break;
            }
            return color;
        }

        private string SetOperationType()
        {
            switch (PendingOperationType)
            {
                case EPendingOperationType.Add: return "Aktion: Hinzufügen";
                case EPendingOperationType.Remove: return "Aktion: Entfernen";
                case EPendingOperationType.Update: return "Aktion: Aktualisieren";
            }
            return string.Empty;
        }
    }
}
