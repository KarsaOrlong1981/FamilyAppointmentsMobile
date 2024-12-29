using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace FamilyAppointmentsMobile.Services   
{
    public class RestServiceClient : IRestClientService
    {
        private readonly ILogger<RestServiceClient> log;
        private HttpClient _httpClient;
       
        public event EventHandler<Appointment> AppointmentsChanged;
        public event EventHandler<TodoList> TodosChanged;
        public event EventHandler<TaskListsChangedEventArgs> TaskListsChanged;
        public event EventHandler TodoChangedNotificationReceived;

        public RestServiceClient()
        {
            log = Ioc.Default.GetService<ILogger<RestServiceClient>>();
        }

        public async Task<bool> ConnectToRestService()
        {
            try
            {
                var success = false;
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

                log.LogInformation("Try to ping Cloud.");
                _httpClient = new HttpClient { BaseAddress = new Uri("https://firestoreiotappointmentsservice-production.up.railway.app") };
                var response = await _httpClient.GetAsync("api/Appointments/ping");
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    log.LogInformation("Pinging cloud successfully");
                    success = true;
                }
               
                return success;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to ping cloud.");
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
                log.LogError(ex, "Error checking YOUVI TCP Service: ");
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

                log.LogInformation("Successfully retrieved all appointments.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching all appointments.");
                throw new ApplicationException("Error fetching all appointments.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching all appointments.");
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

                log.LogInformation($"Successfully retrieved appointments for {member} in {month}/{year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching appointments by month.");
                throw new ApplicationException("Error fetching appointments by month.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching appointments by month.");
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

                log.LogInformation($"Successfully retrieved appointments for {member} on {day}/{month}/{year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching appointments by day.");
                throw new ApplicationException("Error fetching appointments by day.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching appointments by day.");
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

                log.LogInformation($"Successfully retrieved appointments for {member} in {year}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching appointments by year.");
                throw new ApplicationException("Error fetching appointments by year.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching appointments by year.");
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

                log.LogInformation($"Successfully retrieved appointments for member {member}.");
                return appointments;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching appointments for a member.");
                throw new ApplicationException("Error fetching appointments for a member.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching appointments for a member.");
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
                    log.LogInformation("Successfully added a new appointment.");
                    AppointmentsChanged?.Invoke(this, appointment);
                    return true;
                }

                log.LogWarning("Failed to add the appointment. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while adding an appointment.");
                throw new ApplicationException("Error adding the appointment.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while adding an appointment.");
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
                    log.LogInformation("Successfully updated the appointment.");
                    AppointmentsChanged?.Invoke(this, appointment);
                    return true;
                }

                log.LogWarning("Failed to update the appointment. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while updating the appointment.");
                throw new ApplicationException("Error updating the appointment.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while updating the appointment.");
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
                    log.LogInformation($"Successfully deleted appointment with ID: {id}");
                    AppointmentsChanged?.Invoke(this, null);
                    return true;
                }

                log.LogWarning($"Failed to delete appointment with ID: {id}. Status code: " + response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while deleting an appointment.");
                throw new ApplicationException("Error deleting the appointment.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while deleting an appointment.");
                throw;
            }
        }

        public void Dispose()
        {
            log.LogInformation("Disposed.");
            _httpClient.Dispose();
        }

        public async Task<bool> RegisterOrUpdateClient(Clients client)
        {
            try
            {
                log.LogInformation("Register new Client");
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
                log.LogError(ex, "Failed to register or update client");
                return false;
            }
        }

        public void OnAppointmentsChanged(Appointment appointment)
        {
            AppointmentsChanged?.Invoke(this, appointment);
        }

        public async Task<List<TodoList>> GetAllTodoListsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Todo/All");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var todoLists = JsonSerializer.Deserialize<List<TodoList>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<TodoList>();

                log.LogInformation("Successfully retrieved all TodoLists.");
                return todoLists;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while fetching all TodoLists.");
                throw new ApplicationException("Error fetching all TodoLists.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching all TodoLists.");
                throw;
            }
        }

        public async Task<TodoList> GetTodoListByIdAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Todo/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var todoList = JsonSerializer.Deserialize<TodoList>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                log.LogInformation($"Successfully retrieved TodoList with ID {id}.");
                return todoList;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"An error occurred while fetching TodoList with ID {id}.");
                throw new ApplicationException($"Error fetching TodoList with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching TodoList.");
                throw;
            }
        }

        public async Task CreateOrUpdateTodoListAsync(TodoList todoList, ETodoOperationType todoOperationType)
        {
            try
            {
                var json = JsonSerializer.Serialize(todoList);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Todo", content);
                response.EnsureSuccessStatusCode();
                TodosChanged?.Invoke(this, todoList);
                var args = new TaskListsChangedEventArgs { TodoList = todoList, TodoOperationType = todoOperationType };
                TaskListsChanged?.Invoke(this, args);
                log.LogInformation("Successfully created or updated TodoList.");
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while creating or updating TodoList.");
                throw new ApplicationException("Error creating or updating TodoList.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while creating or updating TodoList.");
                throw;
            }
        }

        //Optional
        public async Task<List<TodoTask>> GetTodoTasksByListIdAsync(string todoListId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Todo/Task/ByListId/{todoListId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var todoTasks = JsonSerializer.Deserialize<List<TodoTask>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<TodoTask>();

                log.LogInformation($"Successfully retrieved TodoTasks for TodoList ID {todoListId}.");
                return todoTasks;
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"An error occurred while fetching TodoTasks for TodoList ID {todoListId}.");
                throw new ApplicationException($"Error fetching TodoTasks for TodoList ID {todoListId}.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while fetching TodoTasks.");
                throw;
            }
        }

        //Optional
        public async Task CreateOrUpdateTodoTaskAsync(TodoTask todoTask)
        {
            try
            {
                var json = JsonSerializer.Serialize(todoTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Todo/Task", content);
                response.EnsureSuccessStatusCode();

                log.LogInformation("Successfully created or updated TodoTask.");
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "An error occurred while creating or updating TodoTask.");
                throw new ApplicationException("Error creating or updating TodoTask.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while creating or updating TodoTask.");
                throw;
            }
        }

        public async Task DeleteTodoListAsync(string todoListId, TodoList todoList)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Todo/{todoListId}");
                response.EnsureSuccessStatusCode();
                TodosChanged?.Invoke(this, todoList);
                var args = new TaskListsChangedEventArgs { TodoList = todoList, TodoOperationType = ETodoOperationType.RemoveList };
                TaskListsChanged?.Invoke(this, args);
                log.LogInformation($"Successfully deleted TodoList with ID {todoListId}.");
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"An error occurred while deleting TodoList with ID {todoListId}.");
                throw new ApplicationException($"Error deleting TodoList with ID {todoListId}.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while deleting TodoList.");
                throw;
            }
        }

        //optional
        public async Task DeleteTodoTaskAsync(string todoTaskId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Todo/Task/{todoTaskId}");
                response.EnsureSuccessStatusCode();

                log.LogInformation($"Successfully deleted TodoTask with ID {todoTaskId}.");
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, $"An error occurred while deleting TodoTask with ID {todoTaskId}.");
                throw new ApplicationException($"Error deleting TodoTask with ID {todoTaskId}.", ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error occurred while deleting TodoTask.");
                throw;
            }
        }

        public void OnTodosChangedNotificationReceived()
        {
            TodoChangedNotificationReceived?.Invoke(this, EventArgs.Empty);
        }
    }
}
