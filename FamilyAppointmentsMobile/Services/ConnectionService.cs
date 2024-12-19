using Android.Content.Res;
using Android.Util;
using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAppointmentsMobile.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly IRestClientService _restClientService;

        public bool IsConnected {  get; private set; }

        public event EventHandler<bool> ConnectionChanged;
        public event EventHandler<bool> PendingItemsChanged;

        public ConnectionService()
        {
            _restClientService = Ioc.Default.GetService<IRestClientService>();
            Connectivity.ConnectivityChanged += OnConnectionChanged;
        }

        public async Task<bool> LocalConnection()
        {
            var success = false;
            try
            {
                success = await _restClientService.ConnectToRestService();
                IsConnected = success;
                return success;
            }
            catch
            {
                return false;
            }
            finally
            {
                ConnectionChanged?.Invoke(this, success);
            }
        }

        private async void OnConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;

            if (access == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                var succes = await _restClientService.ConnectToRestService();
                IsConnected = succes;
                ConnectionChanged?.Invoke(this, succes);
                //await HandleNetworkChanged(succes);
            }
            else
            {
                IsConnected = false;
                ConnectionChanged?.Invoke(this, false);
                //Disconnect();
            }
        }

        public void OnPendingItemsChanged(bool pendingItemsChanged)
        {
            PendingItemsChanged?.Invoke(this, pendingItemsChanged);
        }
    }
}
