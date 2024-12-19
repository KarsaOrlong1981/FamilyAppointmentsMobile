using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Models;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using static Android.Provider.CalendarContract;

namespace FamilyAppointmentsMobile.Services
{
    public class AppointmentsTransferService : IAppointmentsTransferService
    {
        private IRestClientService client;
        private IConnectionService connectionService;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);
        private DatabasePendingItems databasePendingItems;
        private DatabaseLocalItems databaseLocalItems;
        private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);
        private bool isConnected;
        public ObservableCollection<Appointment> CurrentAppointments { get; set; }
        public FamilyMember CurrentFamilyMember { get; set; }
        public ObservableCollection<FamilyMember> CurrentFamilyMembers { get; set; }
        public ObservableCollection<PendingAppointment> PendingAppointments { get; set; }

        public bool HasPendingItems {  get; private set; }
        public EventCollection Events { get; private set; }

        public AppointmentsTransferService()
        {
            client = Ioc.Default.GetService<IRestClientService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            databasePendingItems = new DatabasePendingItems();
            databaseLocalItems = new DatabaseLocalItems();
            PendingAppointments = new ObservableCollection<PendingAppointment>();
            isConnected = connectionService.IsConnected;
            connectionService.ConnectionChanged += ConnectionService_ConnectionChanged;
            connectionService.PendingItemsChanged += ConnectionService_PendingItemsChanged;
        }

        private void ConnectionService_PendingItemsChanged(object sender, bool e)
        {
            HasPendingItems = e;
        }

        public async Task CheckForDeques()
        {
            var checkList = await databasePendingItems.GetAllPendingOperationsAsync();
            if (checkList != null && checkList.Count > 0)
            {
                HasPendingItems = true;
            }
            else
            {
                HasPendingItems = false;
            }
        }

        private async void ConnectionService_ConnectionChanged(object? sender, bool e)
        {
            if (!await _connectionSemaphore.WaitAsync(0))
                return;

            try
            {
                isConnected = e;

                if (isConnected)
                {
                    var pendingAppointments = await databasePendingItems.GetAllPendingOperationsAsync();
                    if (pendingAppointments != null && pendingAppointments.Count > 0)
                    {
                        foreach (var appointment in pendingAppointments)
                        {
                            if (appointment.PendingOperationType == EPendingOperationType.Add)
                            {
                                var newAppointment = new Appointment(description: appointment.Description, date: appointment.Date, member: appointment.Member, id: Guid.NewGuid().ToString());
                                await client.AddAppointmentAsync(newAppointment);
                            }
                            else if (appointment.PendingOperationType == EPendingOperationType.Update)
                            {
                                var updatedAppointment = new Appointment(description: appointment.Description, date: appointment.Date, member: appointment.Member, id: appointment.AppointmentId);
                                await client.UpdateAppointmentAsync(updatedAppointment);
                            }
                            else if (appointment.PendingOperationType == EPendingOperationType.Remove)
                            {
                                await client.DeleteAppointmentAsync(appointment.AppointmentId);
                            }
                        }
                        await databasePendingItems.DeleteAllPendingOperationsAsync();
                        PendingAppointments.Clear();
                        connectionService.OnPendingItemsChanged(false);
                        await LoadAppointments();
                    }
                }
                else
                    await LoadAppointments();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _connectionSemaphore.Release();
            }
        }

        public void LoadAppointmentsToday()
        {
            foreach (var member in CurrentFamilyMembers)
            {
                member.AppointmentsToday = GetListofToday(member.Appointments);
            }
        }

        private ObservableCollection<Appointment> GetListofToday(ObservableCollection<Appointment> appointments)
        {
            return appointments?.Where(a => a.Date.Value.Date == DateTime.Now.Date).ToObservableCollection();
        }

        public async Task LoadAppointments()
        {
            await _initSemaphore.WaitAsync();
            try
            {
                List<Appointment> allAppointments;

                if (connectionService.IsConnected)
                {
                    allAppointments = await client.GetAllAppointmentsAsync() ?? new List<Appointment>();

                    // Save appointments locally
                    await databaseLocalItems.SaveAllAppointmentsAsync(allAppointments);
                }
                else
                {
                    var localAppointments = await databaseLocalItems.GetAllLocalAppointmentsAsync();
                    allAppointments = localAppointments
                        .Select(local => new Appointment(local.Description, local.Date, local.Member, local.AppointmentId))
                        .ToList();
                }

                // Process appointments
                if (allAppointments.Any())
                {
                    ProcessAppointments(allAppointments);
                    LoadAppointmentsToday();
                    LoadEvents(allAppointments);
                }
            }
            catch (Exception ex)
            {
                // logger.Error(ex, "Failed to load appointments");
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        private void ProcessAppointments(List<Appointment> appointments)
        {
            // Normalize member names
            foreach (var appointment in appointments)
            {
                if (appointment.Member == "Jörg")
                    appointment.Member = "Joerg";
            }

            // Clear current appointments
            foreach (var member in CurrentFamilyMembers)
            {
                member.Appointments.Clear();
            }

            // Assign appointments to family members
            foreach (var appointment in appointments)
            {
                var member = CurrentFamilyMembers.FirstOrDefault(m => m.Name == appointment.Member);
                member?.Appointments.Add(appointment);
            }

            // Sort appointments
            foreach (var member in CurrentFamilyMembers)
            {
                SortAppointments(member.Appointments);
            }
        }


        private void SortAppointments(ObservableCollection<Appointment> appointments)
        {
            if (appointments == null) return;

            // Temporär sortieren
            var sortedAppointments = appointments.OrderBy(a => a.Date).ToList();

            // ObservableCollection aktualisieren
            appointments.Clear();
            foreach (var appointment in sortedAppointments)
            {
                appointments.Add(appointment);
            }
        }

        private void LoadEvents(List<Appointment> allAppointments)
        {
            try
            {
                if (connectionService.IsConnected)
                {
                   Events = GetAllEvents(allAppointments);
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "Failed to load appointments");
            }
        }

        private EventCollection GetAllEvents(List<Appointment> allAppointments)
        {
            var events = new EventCollection();

            // Termine nach Datum gruppieren
            var groupedAppointments = allAppointments
                .Where(a => a.Date.HasValue)
                .GroupBy(a => a.Date.Value.Date)
                .OrderBy(g => g.Key);

            foreach (var group in groupedAppointments)
            {
                events[group.Key] = group.Select(a => new EventModel
                {
                    Time = a.Date.Value.TimeOfDay,
                    Name = a.Member,
                    Description = a.Description
                }).ToList();
            }

            return events;
        }
    }
}
