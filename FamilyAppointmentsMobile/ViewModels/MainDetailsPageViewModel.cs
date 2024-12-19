using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using System.Collections.ObjectModel;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class MainDetailsPageViewModel : ObservableRecipient
    {
        private IDialogService dialogService;
        private IRestClientService client;
        private IShellNavigationService shellNavigationService;
        private IAppointmentsTransferService appointmentsTransferService;

        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }
        [ObservableProperty] private FamilyMember currentFamilyMember;
        [ObservableProperty] private ObservableCollection<Appointment> currentAppointments;
        [ObservableProperty] private Appointment selectedAppointmentGeneral;
        [ObservableProperty] private ECollectionViewType collectionViewType;
        [ObservableProperty] private ELayoutType layoutType;

        public MainDetailsPageViewModel()
        {
            dialogService = Ioc.Default.GetService<IDialogService>();
            client = Ioc.Default.GetService<IRestClientService>();
            shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            FamilyMembers = appointmentsTransferService.CurrentFamilyMembers;
            CurrentAppointments = appointmentsTransferService.CurrentAppointments;
            CurrentFamilyMember = appointmentsTransferService.CurrentFamilyMember;
            client.AppointmentsChanged += Client_AppointmentsChanged;
        }

        private void Client_AppointmentsChanged(object? sender, Appointment e)
        {
            appointmentsTransferService.LoadAppointments();
            FamilyMembers = appointmentsTransferService.CurrentFamilyMembers;
        }

        [RelayCommand]
        private async Task AddAppointment(string member)
        {
            await dialogService.ShowMopupDialog(EMopUpType.Add, member: member);
        }

        [RelayCommand]
        private void ChangeLayout(ELayoutType layoutType)
        {
            LayoutType = layoutType;
            var newTitle = string.Empty;
            switch (layoutType)
            {
                case ELayoutType.All: newTitle = $"{CurrentFamilyMember.Name}: Alle Termine"; break;
                case ELayoutType.Today: newTitle = $"{CurrentFamilyMember.Name}: Termine Heute"; break;
            }
            shellNavigationService.OnNavigationLayoutChanged(newTitle);
        }

        [RelayCommand]
        private async Task SelectedAppointment()
        {
            if (SelectedAppointmentGeneral != null)
            {
                await dialogService.ShowMopupDialog(EMopUpType.Edit, appointment: SelectedAppointmentGeneral);
                SelectedAppointmentGeneral = null;
            }
        }
    }
}
