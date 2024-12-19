using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAppointmentsMobile.Services
{
    public interface IConnectionService
    {
        event EventHandler<bool> PendingItemsChanged;
        event EventHandler<bool> ConnectionChanged;
        void OnPendingItemsChanged(bool pendingItemsChanged);
        Task<bool> LocalConnection();
        bool IsConnected { get; }
    }
}
