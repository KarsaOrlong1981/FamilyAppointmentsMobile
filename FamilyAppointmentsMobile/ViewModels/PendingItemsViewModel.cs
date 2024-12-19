using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using System.Collections.ObjectModel;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class PendingItemsViewModel : ObservableRecipient
    {
        private IDialogService dialogService;
        private IConnectionService connectionService;
        private IAppointmentsTransferService appointmentsTransferService;
        private DatabasePendingItems outStandingAppointmentsOperations;

        [ObservableProperty] private PendingAppointment selectedPendingItem;
        [ObservableProperty] private ObservableCollection<PendingAppointment> pendingAppointments;

        public PendingItemsViewModel() 
        {
            dialogService = Ioc.Default.GetService<IDialogService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            outStandingAppointmentsOperations = new DatabasePendingItems();
            PendingAppointments = appointmentsTransferService.PendingAppointments;
            connectionService.PendingItemsChanged += ConnectionService_PendingItemsChanged;
        }

        private async void ConnectionService_PendingItemsChanged(object? sender, bool e)
        {
            if (appointmentsTransferService.PendingAppointments != null)
            {
                if (appointmentsTransferService.PendingAppointments?.Count > 0)
                    appointmentsTransferService.PendingAppointments = (await outStandingAppointmentsOperations.GetAllPendingOperationsAsync()).ToObservableCollection();
            }
            else
            {
                appointmentsTransferService.PendingAppointments = (await outStandingAppointmentsOperations.GetAllPendingOperationsAsync()).ToObservableCollection();
            }
        }

        [RelayCommand]
        private async Task SelectedPendingAppointment()
        {
            if (SelectedPendingItem != null)
            {
                var appointmentEvent = new Appointment(description: SelectedPendingItem.Description, date: SelectedPendingItem.Date, member: SelectedPendingItem.Member, id: SelectedPendingItem.AppointmentId);
                await dialogService.ShowMopupDialog(EMopUpType.Edit, appointment: appointmentEvent, isPendingItem: true);
                if (appointmentsTransferService.PendingAppointments.Count > 0)
                {
                    appointmentsTransferService.PendingAppointments = (await outStandingAppointmentsOperations.GetAllPendingOperationsAsync()).ToObservableCollection();
                    PendingAppointments = appointmentsTransferService.PendingAppointments;
                }
                SelectedPendingItem = null;
            }
        }
    }
}
