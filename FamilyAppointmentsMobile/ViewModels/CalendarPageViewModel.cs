using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Services;
using Plugin.Maui.Calendar.Models;
using System.Globalization;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class CalendarPageViewModel : ObservableRecipient
    {
        private IAppointmentsTransferService appointmentsTransferService;

        public CultureInfo CalendarCulture { get; set; } = new CultureInfo("de-DE");
        [ObservableProperty] private EventCollection events;
        
        public CalendarPageViewModel()
        {
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            Events = appointmentsTransferService.Events;
        }
    }
}
