using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Models;
using Microsoft.Extensions.Logging;
using SQLite;

namespace FamilyAppointmentsMobile.Database
{
    public class DatabaseARTevents
    {
        private readonly ILogger<DatabaseARTevents> log;
        private SQLiteAsyncConnection Database;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);


        public DatabaseARTevents() 
        { 
            log = Ioc.Default.GetService<ILogger<DatabaseARTevents>>();
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
                    Database = new SQLiteAsyncConnection(Constants.DatabasePathARTevents, Constants.FlagsARTevents);
                    await Database.CreateTableAsync<ARTEvents>();
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to init Database ART events.");
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        public async Task<List<ARTEvents>> GetAllARTeventsAsync()
        {
            await Init();
            return await Database.Table<ARTEvents>().ToListAsync();
        }

        public async Task SaveAllARTeventsAsync(List<ARTEvents> events)
        {
            if (events == null || !events.Any())
                return;

            await Init();
            await Database.InsertAllAsync(events);           
        }

        public async Task<int> DeleteAllARTeventsAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<ARTEvents>();
        }
    }
}
