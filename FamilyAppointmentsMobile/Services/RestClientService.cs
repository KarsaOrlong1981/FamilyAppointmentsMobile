using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Discovery;
using FamilyAppointmentsMobile.Helpers;
using FamilyAppointmentsMobile.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace FamilyAppointmentsMobile.Services   
{
    public class RestServiceClient : IRestClientService
    {
        private HttpClient _httpClient;
       
        private string _baseUrl;
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<Appointment> AppointmentsChanged;
    
        public RestServiceClient()
        {
        }

        public async Task<bool> ConnectToRestService()
        {
            try
            {
                var success = false;
               
                ////if (_httpClient == null)
                ////{
                //    _httpClient = new HttpClient { BaseAddress = new Uri("http://192.168.1.55:6062") };
                ////}
                //success = await IsTCPServiceAvailable(6062);
                return success;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ConnectToCloud()
        {
            try
            {
                var success = false;
                //https://firestoreiotappointmentsservice-production.up.railway.app/api/appointments/ping
                //if (_httpClient == null)
                //{
                _httpClient = new HttpClient { BaseAddress = new Uri("https://firestoreiotappointmentsservice-production.up.railway.app") };
                var response = await _httpClient.GetAsync("api/Appointments/ping");
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    success = true;
                }
                //}

                return success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Task<bool> IsTCPServiceAvailable(int port)
        {
            try
            {
                var address = System.Net.IPAddress.Parse("192.168.1.55");
                // Create a TCP client to check host/port availability.
                using (var client = new TcpClient())
                {
                    if (client.ConnectAsync(address, port).Wait(2000))
                    {
                        client.Close();
                        return Task.FromResult(true);
                    }
                    else
                        return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                //log.Error($"Error checking YOUVI TCP Service: ", ex);
                return Task.FromResult(false);
            }
        }
       
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Appointments/All");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Appointment>();

                //logger.Info("Successfully retrieved all appointments.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while fetching all appointments.");
                throw new ApplicationException("Error fetching all appointments.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while fetching all appointments.");
                throw;
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByMonthAsync(string member, int year, int month)
        {
            try
            {
                var url = $"api/Appointments/ByMonth?member={Uri.EscapeDataString(member)}&year={year}&month={month}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Appointment>();

                //logger.Info($"Successfully retrieved appointments for {member} in {month}/{year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while fetching appointments by month.");
                throw new ApplicationException("Error fetching appointments by month.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while fetching appointments by month.");
                throw;
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByDayAsync(string member, int year, int month, int day)
        {
            try
            {
                var url = $"api/Appointments/ByDay?member={Uri.EscapeDataString(member)}&year={year}&month={month}&day={day}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Appointment>();

                //logger.Info($"Successfully retrieved appointments for {member} on {day}/{month}/{year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while fetching appointments by day.");
                throw new ApplicationException("Error fetching appointments by day.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while fetching appointments by day.");
                throw;
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByYearAsync(string member, int year)
        {
            try
            {
                var url = $"api/Appointments/ByYear?member={Uri.EscapeDataString(member)}&year={year}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Appointment>();

                //logger.Info($"Successfully retrieved appointments for {member} in {year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while fetching appointments by year.");
                throw new ApplicationException("Error fetching appointments by year.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while fetching appointments by year.");
                throw;
            }
        }

        public async Task<List<Appointment>> GetAppointmentsForMemberAsync(string member)
        {
            try
            {
                var url = $"api/Appointments/ForMember?member={Uri.EscapeDataString(member)}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Appointment>();

                //logger.Info($"Successfully retrieved appointments for member {member}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while fetching appointments for a member.");
                throw new ApplicationException("Error fetching appointments for a member.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while fetching appointments for a member.");
                throw;
            }
        }

        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                var json = JsonSerializer.Serialize(appointment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Appointments", content);
                if (response.IsSuccessStatusCode)
                {
                    //logger.Info("Successfully added a new appointment.");
                    AppointmentsChanged?.Invoke(this, appointment);
                    return true;
                }

                //logger.Warn("Failed to add the appointment. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while adding an appointment.");
                throw new ApplicationException("Error adding the appointment.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while adding an appointment.");
                throw;
            }
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            try
            {
                var json = JsonSerializer.Serialize(appointment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Appointments/{appointment.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    //logger.Info("Successfully updated the appointment.");
                    AppointmentsChanged?.Invoke(this, appointment);
                    return true;
                }

                //logger.Warn("Failed to update the appointment. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while updating the appointment.");
                throw new ApplicationException("Error updating the appointment.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while updating the appointment.");
                throw;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Appointments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    //logger.Info($"Successfully deleted appointment with ID: {id}");
                    AppointmentsChanged?.Invoke(this, null);
                    return true;
                }

                //logger.Warn($"Failed to delete appointment with ID: {id}. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                //logger.Error(ex, "An error occurred while deleting an appointment.");
                throw new ApplicationException("Error deleting the appointment.", ex);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An unexpected error occurred while deleting an appointment.");
                throw;
            }
        }

        public void Dispose()
        {
            //logger.Info("Disposed.");
            _httpClient.Dispose();
        }

        public async Task<bool> RegisterOrUpdateClient(Clients client)
        {
            try
            {
                var json = JsonSerializer.Serialize(client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Client", content);
                if (response.IsSuccessStatusCode)
                {                 
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void OnAppointmentsChanged(Appointment appointment)
        {
            AppointmentsChanged?.Invoke(this, appointment);
        }
    }
}
