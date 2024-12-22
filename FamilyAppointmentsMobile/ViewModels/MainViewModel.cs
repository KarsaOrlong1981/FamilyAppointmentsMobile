using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Services;
using System.Collections.ObjectModel;
using FamilyAppointmentsMobile.Models;
using System.Globalization;
using Plugin.Maui.Calendar.Models;
using FamilyAppointmentsMobile.Helpers;
using CommunityToolkit.Maui.Core.Extensions;
using FamilyAppointmentsMobile.Database;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class MainViewModel : ObservableRecipient
    {
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IDialogService dialogService;
        private IRestClientService client;
        private IConnectionService connectionService;
        private IShellNavigationService shellNavigationService;
        private IAppointmentsTransferService appointmentsTransferService;
        private DatabasePendingItems outStandingAppointmentsOperations;
        private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);

        private DateTime _currentMonth;
        
        [ObservableProperty] private FamilyMember selectedFamilyMember;
        [ObservableProperty] private ObservableCollection<PendingAppointment> pendingAppointments;
        //[ObservableProperty] private ELayoutType layoutType;
        [ObservableProperty] private Appointment selectedAppointmentGeneral;
        [ObservableProperty] private DateTime date;
        [ObservableProperty] private EMembers eMember;
        [ObservableProperty] private ECollectionViewType collectionViewType;
        [ObservableProperty] private bool hasPendingItems;
        [ObservableProperty] private bool isLoading;

        public DateTime CurrentMonth
        {
            get => _currentMonth;
            set => SetProperty(ref _currentMonth, value);
        }

        public ObservableCollection<FamilyMember> FamilyMembers { get; set; }

        public MainViewModel()
        {
            //logger.Info("Start mainViewModel.");
            Date = DateTime.Now;
            dialogService = Ioc.Default.GetService<IDialogService>();
            shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            CurrentMonth = DateTime.Now;
            client = Ioc.Default.GetService<IRestClientService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            PendingAppointments = appointmentsTransferService.PendingAppointments;
            outStandingAppointmentsOperations = new DatabasePendingItems();
            client.AppointmentsChanged += Client_AppointmentsChanged;
            connectionService.ConnectionChanged += ConnectionService_ConnectionChanged;
            connectionService.PendingItemsChanged += ConnectionService_PendingItemsChanged; ;
            // Initialize Family Members
            FamilyMembers = new ObservableCollection<FamilyMember>
            {
                new FamilyMember("Karin", ResourceHelper.GetResource<Color>("KarinColor")),
                new FamilyMember("Joerg", ResourceHelper.GetResource<Color>("JoergColor")),
                new FamilyMember("Marvin", ResourceHelper.GetResource < Color >("MarvinColor")),
                new FamilyMember("Lio", ResourceHelper.GetResource < Color >("LioColor"))
            };
            appointmentsTransferService.CurrentFamilyMembers = FamilyMembers;

            try
            {
                DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                {
                    //var localConnection = await connectionService.LocalConnection();
                    //if (!localConnection)
                    //{
                        var cloudConnection = await connectionService.CloudConnection();
                        if (!cloudConnection)
                        {

                        }
                    //}
                    
                    await appointmentsTransferService.LoadAppointments();
                    //await appointmentsTransferService.CheckForDeques();
                });
            }
            catch (Exception ex) 
            {

            }
        }

        private void ConnectionService_PendingItemsChanged(object sender, bool e)
        {
            HasPendingItems = e;
        }

        private void ConnectionService_ConnectionChanged(object? sender, EConnectionType connectionType)
        {
            appointmentsTransferService.LoadAppointments();
        }

        private void Client_AppointmentsChanged(object? sender, Appointment e)
        {
           appointmentsTransferService.LoadAppointments();
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

        [RelayCommand]
        private async Task SelectedFamilyMemberAction()
        {
            if (SelectedFamilyMember != null)
            {
                appointmentsTransferService.CurrentFamilyMember = SelectedFamilyMember;
                appointmentsTransferService.CurrentAppointments = SelectedFamilyMember?.Appointments;
              
                switch (appointmentsTransferService.CurrentFamilyMember.Name)
                {
                    case "Joerg": EMember = EMembers.Joerg; break;
                    case "Karin": EMember = EMembers.Karin; break;
                    case "Marvin": EMember = EMembers.Marvin; break;
                    case "Lio": EMember = EMembers.Lio; break;
                }

                await shellNavigationService.NavigateTo(Constants.MainDetailsPage);
                shellNavigationService.OnNavigationLayoutChanged(SelectedFamilyMember.Name + ": Alle Termine");
                SelectedFamilyMember = null;
            }
        }

        [RelayCommand]
        private async Task ChangeLayout(ELayoutType layoutType)
        {
            if (layoutType == ELayoutType.Calendar)
            {
                await shellNavigationService.NavigateTo(Constants.CalendarPage);
            }
        }

        [RelayCommand]
        private async Task OpenPendingItems()
        {
            appointmentsTransferService.PendingAppointments = (await outStandingAppointmentsOperations.GetAllPendingOperationsAsync()).ToObservableCollection();
            await shellNavigationService.NavigateTo(Constants.PendingItemsPage);
        }
    }
}
