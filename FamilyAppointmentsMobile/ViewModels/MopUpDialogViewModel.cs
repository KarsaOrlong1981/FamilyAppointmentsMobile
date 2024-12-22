using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using Mopups.Interfaces;
using System.ComponentModel;


namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class MopUpDialogViewModel : ObservableRecipient
    {
        private readonly IPopupNavigation popupNavigation;
        private readonly IConnectionService connectionService;
        private readonly TaskCompletionSource<bool> result;
        private IRestClientService restClientService;
        private IDialogService dialogService;
        private RelayCommand closeDialogCommand;
        private Appointment equalAppointment;
        private DatabasePendingItems databasePendingItems;
        private bool isPendingItem;

        [ObservableProperty]
        private string message;
        [ObservableProperty]
        private string title;
        [ObservableProperty]
        private EMopUpType type;
       
        public MopUpDialogViewModel(IPopupNavigation popupNavigation, TaskCompletionSource<bool> tcs, EMopUpType mopUpType, string title = "", string message = "", string member = "", Appointment appointment = null, bool isPendingItem = false) 
        {
            restClientService = Ioc.Default.GetService<IRestClientService>();
            dialogService = Ioc.Default.GetService<IDialogService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            databasePendingItems = new DatabasePendingItems();
            
            if (appointment != null)
            {
                equalAppointment = appointment;
                EventText = appointment.Description;
                Date = appointment.Date.Value.Date;
                Time = appointment.Date.Value.TimeOfDay;
            }
            else
            {
                Time = CurrentTime();
            }
            
            this.type = mopUpType;
            this.result = tcs;
            this.popupNavigation = popupNavigation;
            this.title = title;
            this.message = message;
            this.member = member;
            this.appointment = appointment;
            this.isPendingItem = isPendingItem;
        }

        public RelayCommand CloseDialogCommand => closeDialogCommand ?? (closeDialogCommand = new RelayCommand(async () => await CloseDialogAction()));

        private async Task CloseDialogAction()
        {
            result.SetResult(false);
            await popupNavigation.PopAsync();
        }

        [RelayCommand]
        private async Task Answer(EAnswerOptions answerOptions)
        {
            var res = false;
            switch (answerOptions)
            {
                case EAnswerOptions.Ok: res = true; break;
                case EAnswerOptions.Cancel: res = false; break;
                case EAnswerOptions.Yes: res = true; break;
                case EAnswerOptions.No: res = false; break;
            }
            result.SetResult(res);
            await popupNavigation.PopAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            result.SetResult(false);
            await popupNavigation.PopAsync();
        }

        #region Add Dialog
        [ObservableProperty]
        private string eventText;

        [ObservableProperty]
        private DateTime? date = DateTime.Now;

        [ObservableProperty]
        private string member;

        [ObservableProperty]
        private TimeSpan time;

        private TimeSpan CurrentTime()
        {
            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;
            return new TimeSpan(hour, minute, 0);
        }

        public bool IsValid => !string.IsNullOrEmpty(EventText);
        public bool CanUpdate => EqualAppointmentCanUpdate();

        private bool EqualAppointmentCanUpdate()
        {
            if (Date.Value.Date == Appointment.Date.Value.Date && Time == Appointment.Date.Value.TimeOfDay && EventText == Appointment.Description)
                return false;
            else
                return true;
        }
        
        [RelayCommand]
        private async Task Add()
        {
            try
            {
                var combinedDateTime = Date.Value.Date + Time;
                var appointmentEvent = new Appointment(description: EventText, date: combinedDateTime, member: Member, id: Guid.NewGuid().ToString());

                //if (connectionService.IsConnected)
                //{
                    await restClientService.AddAppointmentAsync(appointmentEvent);
                //}
                //else
                //{
                //    await databasePendingItems.SavePendingOperationAsync(appointmentEvent, EPendingOperationType.Add);
                //    await dialogService.ShowMopupDialog(EMopUpType.Message, "Nicht Verbunden !!!", "Der neue Eintrag wurde nun zwichen gespeichert und wird erst hinzugefügt wenn die Verbindung wieder hergestellt wurde.");
                //    connectionService.OnPendingItemsChanged(true);
                //}

                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch(Exception ex)
            {

            }
        }

        [RelayCommand]
        private void Reset(EMopUpType mopUpType)
        {
            if (mopUpType == EMopUpType.Add)
            {
                Date = DateTime.Now;
                EventText = string.Empty;
                var hour = DateTime.Now.Hour;
                var minute = DateTime.Now.Minute;

                Time = new TimeSpan(hour, minute, 0);
            }
            else if (mopUpType == EMopUpType.Edit)
            {
                Date = Appointment.Date.Value.Date;
                Time = Appointment.Date.Value.TimeOfDay;
                EventText = Appointment.Description;
            }
            
        }

        [RelayCommand]
        private void TextChanged()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(CanUpdate));
        }

     

        [RelayCommand]
        private void PropertyChanged(EventArgs e)
        {
            if (e is PropertyChangedEventArgs args)
            {
                if (args.PropertyName == "Time" || args.PropertyName == "Date")
                {
                    OnPropertyChanged(nameof(CanUpdate));
                }
            }
        }

        #endregion

        #region Edit
        [ObservableProperty]
        private Appointment appointment;

        [RelayCommand]
        private async Task Update()
        {
            try
            {
                Appointment.Description = EventText;
                Appointment.Date = Date.Value.Date + Time;

                //if (connectionService.IsConnected)
                //{
                    await restClientService.UpdateAppointmentAsync(Appointment);
                //}
                //else if (!isPendingItem)
                //{
                //    await databasePendingItems.SavePendingOperationAsync(Appointment, EPendingOperationType.Update);
                //    await dialogService.ShowMopupDialog(EMopUpType.Message, "Nicht Verbunden !!!", "Die Überarbeitung wurde nun zwichen gespeichert und wird erst aktuallisiert wenn die Verbindung wieder hergestellt wurde.");
                //    connectionService.OnPendingItemsChanged(true);
                //}
                //else if (isPendingItem)
                //{
                //    // update database item
                //    await databasePendingItems.UpdateByAppointmentIdAsync(Appointment.Id, Appointment);
                //}

                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch(Exception ex)
            {

            }
            
        }

        [RelayCommand]
        private async Task Remove()
        {
            try
            {
                //if (connectionService.IsConnected)
                //{
                    var res = await dialogService.ShowMopupDialog(EMopUpType.YesNo, "Achtung !!!", "Sicher das dieser Eintrag gelöscht werden soll?");

                    if (res)
                    {
                        await restClientService.DeleteAppointmentAsync(Appointment.Id);
                    }
                //}
                //else if (!isPendingItem)
                //{
                //    await databasePendingItems.SavePendingOperationAsync(Appointment, EPendingOperationType.Remove);
                //    await dialogService.ShowMopupDialog(EMopUpType.Message, "Nicht Verbunden !!!", "Der gelöschte Eintrag wird nun zwichen gespeichert und erst entfernt wenn die Verbindung wieder hergestellt wurde.");
                //    connectionService.OnPendingItemsChanged(true);
                //}
                //else if (isPendingItem)
                //{
                //    await databasePendingItems.DeleteByAppointmentIdAsync(Appointment.Id);
                //}
                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch (Exception ex)
            {

            } 
        }
        #endregion
    }
}
