

using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Services
{
    public interface IConnectionService
    {
        event EventHandler<bool> PendingItemsChanged;
        event EventHandler<EConnectionType> ConnectionChanged;
        void OnPendingItemsChanged(bool pendingItemsChanged);
        Task<bool> LocalConnection();
        Task<bool> CloudConnection();   
        bool IsConnected { get; }
    }
}
