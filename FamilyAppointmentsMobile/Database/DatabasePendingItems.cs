using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using SQLite;

namespace FamilyAppointmentsMobile.Database
{
    public class DatabasePendingItems
    {
        private IConnectionService connectionService;
        private SQLiteAsyncConnection Database;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);

        public DatabasePendingItems()
        {
            connectionService = Ioc.Default.GetService<IConnectionService>();
        }

        private async Task Init()
        {
            if (Database is not null)
                return;

            await _initSemaphore.WaitAsync();
            try
            {
                if (Database is null)
                {
                    Database = new SQLiteAsyncConnection(Constants.DatabasePathPending, Constants.FlagsPending);
                        await Database.CreateTableAsync<PendingAppointment>();
                }
            }
            catch (Exception ex)
            {
                //log.LogError(ex, "Failed to init Database.");
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        public async Task<List<PendingAppointment>> GetAllPendingOperationsAsync()
        {
            await Init();
            return await Database.Table<PendingAppointment>().ToListAsync();
        }

        public async Task SavePendingOperationAsync(Appointment appointment, EPendingOperationType operationType)
        {
            var succes = false;
            try
            {
                if (appointment != null)
                {
                    var outstandingAppointment = new PendingAppointment
                    {
                        AppointmentId = appointment.Id,
                        Description = appointment.Description,
                        Member = appointment.Member,
                        Date = appointment.Date,
                        PendingOperationType = operationType
                    };

                    await AddPendingOperationAsync(outstandingAppointment);
                    succes = true;
                }
            }
            catch (Exception ex)
            {

            }  
        }

        private async Task<int> AddPendingOperationAsync(PendingAppointment newAppointment)
        {
            if (newAppointment == null)
                throw new ArgumentNullException(nameof(newAppointment));

            await Init();
            return await Database.InsertAsync(newAppointment);
        }

        private async Task<int> UpdatePendingOperationAsync(PendingAppointment existingAppointment)
        {
            if (existingAppointment == null)
                throw new ArgumentNullException(nameof(existingAppointment));

            await Init();
            return await Database.UpdateAsync(existingAppointment);
        }

        public async Task<int> DeletePendingItemAsync(PendingAppointment item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> DeleteByAppointmentIdAsync(string appointmentId)
        {
            try
            {
                await Init();
                return await Database.ExecuteAsync($"DELETE FROM PendingAppointment WHERE AppointmentId = ?", appointmentId);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                var hasItems = await GetAllPendingOperationsAsync();
                if (!hasItems.Any())
                    connectionService.OnPendingItemsChanged(false);
            }
           
        }

        public async Task<int> UpdateByAppointmentIdAsync(string appointmentId, Appointment appointment)
        {
            if (string.IsNullOrEmpty(appointmentId))
                throw new ArgumentNullException(nameof(appointmentId));

            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            var updatedAppointment = new PendingAppointment
            {
                AppointmentId = appointment.Id,
                Description = appointment.Description,
                Member = appointment.Member,
                Date = appointment.Date
            };

            try
            {
                await Init();

                // SQL-Update-Statement
                string query = @"
            UPDATE PendingAppointment 
            SET 
                Member = ?, 
                Description = ?, 
                Date = ?
            WHERE AppointmentId = ?";

                // Werte an die Parameter binden
                return await Database.ExecuteAsync(query,
                    updatedAppointment.Member,
                    updatedAppointment.Description,
                    updatedAppointment.Date,
                    appointmentId);
            }
            catch (Exception ex)
            {
                // Fehler loggen und ggf. speziellen Rückgabewert setzen
                
                return 0;
            }
        }

        public async Task<int> DeleteAllPendingOperationsAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<PendingAppointment>();
        }
    }
}
