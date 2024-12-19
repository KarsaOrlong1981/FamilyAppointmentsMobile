using FamilyAppointmentsMobile.Models;
using SQLite;

namespace FamilyAppointmentsMobile.Database
{
    public class DatabaseLocalItems
    {
        private SQLiteAsyncConnection Database;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);

        private async Task Init()
        {
            if (Database is not null)
                return;

            await _initSemaphore.WaitAsync();
            try
            {
                if (Database is null)
                {
                    Database = new SQLiteAsyncConnection(Constants.DatabasePathLocal, Constants.FlagsLocal);
                    await Database.CreateTableAsync<LocalAppointment>();
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

        public async Task<List<LocalAppointment>> GetAllLocalAppointmentsAsync()
        {
            await Init();
            return await Database.Table<LocalAppointment>().ToListAsync();
        }

        public async Task SaveAllAppointmentsAsync(List<Appointment> appointments)
        {
            if (appointments == null || !appointments.Any())
                return;

            await Init();

            // Hole alle existierenden IDs aus der Datenbank
            var existingIds = (await Database.Table<LocalAppointment>().ToListAsync())
                .Select(a => a.AppointmentId)
                .ToHashSet();

            // Filtere neue Einträge basierend auf den vorhandenen IDs
            var newAppointments = appointments
                .Where(a => !existingIds.Contains(a.Id))
                .Select(a => new LocalAppointment
                {
                    AppointmentId = a.Id,
                    Description = a.Description,
                    Member = a.Member,
                    Date = a.Date
                })
                .ToList();

            if (newAppointments.Any())
            {
                // Füge alle neuen Einträge in einem Batch hinzu
                await Database.InsertAllAsync(newAppointments);
            }
        }

        public async Task<int> DeleteAllLocalAppointmentsAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<LocalAppointment>();
        }
    }
}
