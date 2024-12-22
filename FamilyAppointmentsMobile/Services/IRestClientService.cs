using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Services
{
    public interface IRestClientService
    {
        event EventHandler<Appointment> AppointmentsChanged;
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
        void OnAppointmentsChanged(Appointment appointment);
    }
}
