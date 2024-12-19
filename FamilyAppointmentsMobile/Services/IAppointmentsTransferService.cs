using FamilyAppointmentsMobile.Models;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;

namespace FamilyAppointmentsMobile.Services
{
    public interface IAppointmentsTransferService
    {
        ObservableCollection<Appointment> CurrentAppointments { get; set; }
        FamilyMember CurrentFamilyMember { get; set; }
        EventCollection Events { get; }
        ObservableCollection<FamilyMember> CurrentFamilyMembers { get; set; }
        ObservableCollection<PendingAppointment> PendingAppointments { get; set; }
        bool HasPendingItems { get; }
        void LoadAppointmentsToday();
        Task LoadAppointments();
        Task CheckForDeques();
    }
}
