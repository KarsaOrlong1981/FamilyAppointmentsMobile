

using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Services
{
    public interface IART_Service
    {
        Task LoadAndUpdateARTEventsAsync();
        List<ARTEvents> ART_Events { get; set; }
    }
}
