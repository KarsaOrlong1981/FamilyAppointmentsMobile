using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Database;
using FamilyAppointmentsMobile.Helpers;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using Microsoft.Extensions.Logging;
using Mopups.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class MopUpDialogViewModel : ObservableRecipient
    {
        private readonly ILogger<MopUpDialogViewModel> log;
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
       
        public MopUpDialogViewModel(IPopupNavigation popupNavigation, TaskCompletionSource<bool> tcs, EMopUpType mopUpType, string title = "", string message = "", string member = "", Appointment appointment = null, bool isPendingItem = false, TodoList todos = null) 
        {
            restClientService = Ioc.Default.GetService<IRestClientService>();
            dialogService = Ioc.Default.GetService<IDialogService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            log = Ioc.Default.GetService<ILogger<MopUpDialogViewModel>>();
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
            TodoTasks = new ObservableCollection<TodoTask>();
            SelectedCollection = new ObservableCollection<object>();
            DefinedTaskCollection = TodoListHelper.GetSortedPredefiendShoppingItemsList(RemoveTodoTask);
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
            else if (mopUpType == EMopUpType.AddList)
            {
                ListName = string.Empty;
                TodoTasks.Clear();
                CustomItemDescription = string.Empty;
                SelectedCollection.Clear();
                CanChooseItems = false;
                OnPropertyChanged(nameof(TodosHasItems));
            }
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

                await restClientService.AddAppointmentAsync(appointmentEvent);

                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Error on adding new appointment event.");
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

                await restClientService.UpdateAppointmentAsync(Appointment);

                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Error on upadting appointment event");
            }
            
        }

        [RelayCommand]
        private async Task Remove()
        {
            try
            {
                var res = await dialogService.ShowMopupDialog(EMopUpType.YesNo, "Achtung !!!", "Sicher das dieser Eintrag gelöscht werden soll?");

                if (res)
                {
                    await restClientService.DeleteAppointmentAsync(Appointment.Id);
                }
           
                result.SetResult(false);
                await popupNavigation.PopAllAsync();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on removing appointment event.");
            } 
        }
        #endregion

        #region Add new List

        public bool ListNameIsValid => !string.IsNullOrEmpty(ListName);
        public bool CustomItemDescIsValid => !string.IsNullOrEmpty(CustomItemDescription);

        [ObservableProperty] private string listName;
        [ObservableProperty] private ObservableCollection<TodoTask> todoTasks;
        [ObservableProperty] private List<TodoTask> definedTaskCollection;
        [ObservableProperty] private ObservableCollection<object> selectedCollection;
        [ObservableProperty] private bool canChooseItems;
        [ObservableProperty] private string chooseItemsButtonText = "Vorschläge anzeigen";
        [ObservableProperty] private string customItemDescription;
        [ObservableProperty] private bool isShoppingList = true;

        public bool TodosHasItems => TodoTasks.Count > 0;

        private void RemoveTodoTask(TodoTask task)
        {
            if (task != null)
            {
                TodoTasks.Remove(task);

                var selectedItem = SelectedCollection.FirstOrDefault(item => item == task);
                if (selectedItem != null)
                {
                    SelectedCollection.Remove(selectedItem);
                }
               
                OnPropertyChanged(nameof(TodosHasItems));
            }
        }

        [RelayCommand]
        private void ListNameChanged()
        {
            OnPropertyChanged(nameof(ListNameIsValid));
        }

        [RelayCommand]
        private async Task AddNewList()
        {
            try
            {
                var newList = new TodoList
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ListName,
                    Todos = TodoTasks.ToList(),
                    IsShoppingList = IsShoppingList
                };

                foreach (var task in newList.Todos)
                {
                    task.TodoListId = newList.Id;
                }

                await restClientService.CreateOrUpdateTodoListAsync(newList, ETodoOperationType.AddList);
                result.SetResult(true);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on saving new list.");
                await dialogService.ShowMopupDialog(EMopUpType.Message, "Fehler", $"Es gab ein Problem beim Speichern der Liste: {ex.Message}");
                result.SetResult(false);
            }
            finally
            {
                await popupNavigation.PopAllAsync();
            }
        }

        [RelayCommand]
        private async Task ChooseItems()
        {
            CanChooseItems = !CanChooseItems;
            ChooseItemsButtonText = CanChooseItems ? "Vorschläge nicht mehr anzeigen" : "Vorschläge anzeigen";
        }

        [RelayCommand]
        private void SelectionChanged()
        {
            foreach (var item in SelectedCollection.Cast<TodoTask>())
            {
                if (!TodoTasks.Contains(item))
                {
                    TodoTasks.Add(item);
                }
            }
            
            OnPropertyChanged(nameof(TodosHasItems));
        }

        [RelayCommand]
        private void CustomItemDescChanged()
        {
            OnPropertyChanged(nameof(CustomItemDescIsValid));
        }

        [RelayCommand]
        private async Task AddNewItem()
        {
            try
            {
                var result = Constants.Sonstiges;

                if (IsShoppingList)
                    result = await dialogService.ShowCategoriePickerMopupDialog();

                if (TodoTasks.Any(task => task.Description.Equals(CustomItemDescription, StringComparison.OrdinalIgnoreCase)))
                {
                    return;
                }

                var newItem = new TodoTask(RemoveTodoTask)
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = CustomItemDescription,
                    CategorieType = CategorieHelper.HandleStringCategorieType(result),
                    IsDone = false
                };

                TodoTasks.Add(newItem);
                CustomItemDescription = string.Empty;

                OnPropertyChanged(nameof(TodosHasItems));
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Error on adding new item.");
            }
        }
        #endregion
    }
}
