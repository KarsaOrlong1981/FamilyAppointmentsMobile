using System.Collections.ObjectModel;

namespace FamilyAppointmentsMobile.Models 
{
    public class FamilyMember
    {
        public string Name { get; }
        public ObservableCollection<Appointment> Appointments { get; }

        public ObservableCollection<Appointment> AppointmentsToday { get; set; }
        public Color MemberColor { get; set; }
        public FamilyMember(string name, Color memberColor)
        {
            Name = name;
            MemberColor = memberColor;
            Appointments = new ObservableCollection<Appointment>();
            AppointmentsToday = new ObservableCollection<Appointment>();
           
        }
    }
}
