using FamilyAppointmentsMobile.Helpers;
using System;
using System.Text.Json.Serialization;   

namespace FamilyAppointmentsMobile.Models 
{
    public class Appointment
    {
        private Random rnd = new Random();

        public string Id { get; set; }
        public string Member { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public Color Color { get; set; }

        [JsonIgnore]
        public string PostItSource { get; set; }

        [JsonIgnore]
        public string TimeFormatted => SetTimeFormat();

        private string SetTimeFormat()
        {
            if (Date.HasValue)
            {
                return $"{Date.Value.TimeOfDay.Hours:D2}:{Date.Value.TimeOfDay.Minutes:D2} Uhr";
            }
            else
                return string.Empty ;
        }

        [JsonIgnore]
        public string AppointmentDay
        {
            get
            {
                return Date.HasValue
                    ? Date.Value.ToString("dddd", new System.Globalization.CultureInfo("de-DE"))
                    : "K.A.";
            }
        }

        [JsonIgnore]
        public string FormattedDate 
        { 
            get
            {
                return Date.HasValue ? $"{Date.Value:dd.MM.yyyy}" : "K.A.";
            } 
        }

        

        public Appointment(string description, DateTime? date, string member, string id)
        {
            Id = id;
            Description = description;
            Date = date;
            Member = member;
            PostItSource = SetPostIt();
        }

        private string SetPostIt()
        {
            var count = rnd.Next(1, 9);
            var postIt = $"postit{count}.png";
            return postIt ;
        }

    }
}
