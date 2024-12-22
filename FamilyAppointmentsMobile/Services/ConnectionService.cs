using Android.Content.Res;
using Android.Util;
using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Models;
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

        public event EventHandler<EConnectionType> ConnectionChanged;
        public event EventHandler<bool> PendingItemsChanged;

        public ConnectionService()
        {
            _restClientService = Ioc.Default.GetService<IRestClientService>();
            Connectivity.ConnectivityChanged += OnConnectionChanged;
        }

        public async Task<bool> LocalConnection()
        {
            var success = false;
            var connectionType = EConnectionType.NotConnected;
            try
            {
                success = await _restClientService.ConnectToRestService();
                IsConnected = success;
                if (success) 
                    connectionType = EConnectionType.Local;
                return success;
            }
            catch(Exception ex) 
            {
                connectionType = EConnectionType.NotConnected;
                return false;
            }
            finally
            {
                ConnectionChanged?.Invoke(this, connectionType);
            }
        }

        private async void OnConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;

            if (access == Microsoft.Maui.Networking.NetworkAccess.Internet)
            {
                //var localConnection = await _restClientService.ConnectToRestService();
                ////IsConnected = localConnection;
                //if (localConnection)
                //{
                //    IsConnected = true;
                //    ConnectionChanged?.Invoke(this, EConnectionType.Local);
                //}
                //else
                //{
                    var cloudConnection = await _restClientService.ConnectToCloud();
                    if (cloudConnection)
                    {
                        IsConnected = true;
                        ConnectionChanged?.Invoke(this, EConnectionType.Cloud);
                    }
                //}
            }
            else
            {
                IsConnected = false;
                ConnectionChanged?.Invoke(this, EConnectionType.NotConnected);
            }
        }

        public void OnPendingItemsChanged(bool pendingItemsChanged)
        {
            PendingItemsChanged?.Invoke(this, pendingItemsChanged);
        }

        public async Task<bool> CloudConnection()
        {
            var connectionType = EConnectionType.NotConnected;
            try
            {
                var succes = await _restClientService.ConnectToCloud();
                IsConnected = succes;
                if (succes)
                    connectionType = EConnectionType.Cloud;
                return succes;
            }
            catch (Exception ex) 
            {
                connectionType = EConnectionType.NotConnected;
                return false;
            }
            finally
            {
                ConnectionChanged?.Invoke(this, connectionType);
            }

        }
    }
}
