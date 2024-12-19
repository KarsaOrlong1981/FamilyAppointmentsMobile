
using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Services
{
    public interface IDialogService
    {
        Task<bool> ShowMopupDialog(EMopUpType mopUpType, string title = "", string message = "", string member = "", Appointment appointment = null, bool isPendingItem = false);
    }
}
