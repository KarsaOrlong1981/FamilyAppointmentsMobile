using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Services
{
    public interface IRestClientService
    {
        event EventHandler<Appointment> AppointmentsChanged;
        event EventHandler TodosChanged;
        Task<bool> ConnectToRestService();
        Task<bool> ConnectToCloud();
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<List<Appointment>> GetAppointmentsByMonthAsync(string member, int year, int month);
        Task<List<Appointment>> GetAppointmentsByDayAsync(string member, int year, int month, int day);
        Task<List<Appointment>> GetAppointmentsByYearAsync(string member, int year);
        Task<List<Appointment>> GetAppointmentsForMemberAsync(string member);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> AddAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(string id);
        Task<bool> RegisterOrUpdateClient(Clients client);
        Task<List<TodoList>> GetAllTodoListsAsync();
        Task<TodoList> GetTodoListByIdAsync(string id);
        Task CreateOrUpdateTodoListAsync(TodoList todoList);
        Task<List<TodoTask>> GetTodoTasksByListIdAsync(string todoListId);
        Task CreateOrUpdateTodoTaskAsync(TodoTask todoTask);
        Task DeleteTodoListAsync(string todoListId);
        Task DeleteTodoTaskAsync(string todoTaskId);
        void OnAppointmentsChanged(Appointment appointment);
        void OnTodosChanged();

    }
}
