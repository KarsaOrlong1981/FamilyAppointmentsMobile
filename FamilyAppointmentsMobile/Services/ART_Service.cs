using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Platforms.Droid.Services;
using Ical.Net;
using Microsoft.Extensions.Logging;


namespace FamilyAppointmentsMobile.Services
{
    public class ART_Service : IART_Service
    {
        private readonly ILogger<ART_Service> log;
        private const string BaseFile = "54424_Thalfang___01-1800.ics";
        private readonly DatabaseARTevents artEventsDatabase;
        private FirebaseService firebaseService;

        public List<ARTEvents> ART_Events { get; set; }

        public ART_Service()
        {
            log = Ioc.Default.GetService<ILogger<ART_Service>>();
            artEventsDatabase = new DatabaseARTevents();
            firebaseService = new FirebaseService();
        }

        public async Task LoadAndUpdateARTEventsAsync()
        {
            string savePath = Path.Combine(FileSystem.AppDataDirectory, BaseFile);
            await DownloadIcsFileAsync(savePath);
            ART_Events = ParseIcsFile(savePath) ?? new List<ARTEvents>();
        }

        private async Task DownloadIcsFileAsync(string savePath)
        {
            string url = "https://art-trier.de/ics-feed/54424:Thalfang::@01-1800.ics";
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    byte[] icsData = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(savePath, icsData);
                    log.LogInformation($"ICS-Datei heruntergeladen und gespeichert unter: {savePath}");
                }
                else
                {
                    log.LogInformation($"Fehler beim Herunterladen der ICS-Datei: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on Downloading ART events");
            }
        }

        private List<ARTEvents> ParseIcsFile(string savePath)
        {
            try
            {
                var calendar = Calendar.Load(File.ReadAllText(savePath));
                List<ARTEvents> eventsWithStartTimes = new List<ARTEvents>();

                foreach (var calendarEvent in calendar.Events)
                {
                    var artEvent = new ARTEvents
                    {
                        EventName = calendarEvent.Summary,
                        StartTime = calendarEvent.Start.AsSystemLocal
                    };
                    artEvent.Year = artEvent.StartTime.Year;
                    eventsWithStartTimes.Add(artEvent);
                    firebaseService.ScheduleNotification(artEvent);
                }

                return eventsWithStartTimes;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error parsing ICS file");
                return new List<ARTEvents>();
            }
        }

        
    }
}
