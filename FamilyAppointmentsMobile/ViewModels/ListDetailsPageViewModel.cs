using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Helpers;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class ListDetailsPageViewModel : ObservableRecipient
    {
        private readonly ILogger<ListDetailsPageViewModel> log;
        private IDialogService _dialogService;
        private IRestClientService _restClientService;
        private IShellNavigationService _shellNavigationService;
        private IAppointmentsTransferService _appointmentsTransferService;
        private List<TodoTaskGroup> _originalTasksGroups;
        private TodoList currentList;

        public bool HasChanges => CheckHasChanges();
        public bool NewEntryIsValid => !string.IsNullOrEmpty(NewEntry);

        [ObservableProperty] private ObservableCollection<TodoTaskGroup> tasksGroups;
        [ObservableProperty] private ObservableCollection<TodoTask> tasks;
        [ObservableProperty] private string listName;
        [ObservableProperty] private bool isShoppingList;
        [ObservableProperty] private bool isAddingActive;
        [ObservableProperty] private string newEntry;

        public ListDetailsPageViewModel() 
        {
            log = Ioc.Default.GetService<ILogger<ListDetailsPageViewModel>>();
            _dialogService = Ioc.Default.GetService<IDialogService>();
            _shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            _appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();  
            _restClientService = Ioc.Default.GetService<IRestClientService>();
            currentList = _appointmentsTransferService?.CurrentTodoList;

            if (currentList != null )
            {    
                if (currentList.IsShoppingList)
                {
                    IsShoppingList = true;
                    TasksGroups = GroupItems(currentList).ToObservableCollection();
                    _originalTasksGroups = CloneList(TasksGroups);
                }
                else
                {
                    IsShoppingList = false;
                    ListName = currentList.Name;
                    Tasks = currentList.Todos.ToObservableCollection();
                }         
            }
           
        }

        private List<TodoTaskGroup> GroupItems(TodoList list)
        {
            var tasks = list.Todos;
            var newGroups = tasks
                .GroupBy(task => task.CategorieType) // Gruppieren nach CategorieType
                .Select(group => new TodoTaskGroup(group.Key, group.ToList())) 
                .ToList();
            return newGroups;
        }

        private List<TodoTaskGroup> CloneList(ObservableCollection<TodoTaskGroup> originalList)
        {
            return originalList
                .Select(group => new TodoTaskGroup(group.CategorieType, group.Tasks.Select(task => new TodoTask
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsDone = task.IsDone,
                    // Weitere Eigenschaften hier kopieren
                }).ToList()))
                .ToList();
        }

        private bool CheckHasChanges()
        {
            if (IsShoppingList)
            {
                if (_originalTasksGroups.Count != TasksGroups.Count)
                    return true;

                for (int i = 0; i < _originalTasksGroups.Count; i++)
                {
                    var originalGroup = _originalTasksGroups[i];
                    var currentGroup = TasksGroups[i];

                    if (originalGroup.CategorieType != currentGroup.CategorieType)
                        return true;

                    if (originalGroup.Tasks.Count != currentGroup.Tasks.Count)
                        return true;

                    for (int j = 0; j < originalGroup.Tasks.Count; j++)
                    {
                        var originalTask = originalGroup.Tasks[j];
                        var currentTask = currentGroup.Tasks[j];

                        if (originalTask.Id != currentTask.Id ||
                            originalTask.Description != currentTask.Description ||
                            originalTask.IsDone != currentTask.IsDone)
                        {
                            return true;
                        }
                    }
                }
            }
            else // Normale To-Do-Liste
            {
                if (_appointmentsTransferService.CurrentTodoList.Todos.Count != Tasks.Count)
                    return true;

                for (int i = 0; i < _appointmentsTransferService.CurrentTodoList.Todos.Count; i++)
                {
                    var originalTask = _appointmentsTransferService.CurrentTodoList.Todos[i];
                    var currentTask = Tasks[i];

                    if (originalTask.Id != currentTask.Id ||
                        originalTask.Description != currentTask.Description ||
                        originalTask.IsDone != currentTask.IsDone)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        [RelayCommand]
        private void NewEntryChanged()
        {
            OnPropertyChanged(nameof(NewEntryIsValid));
        }

        [RelayCommand]
        private void CheckedChanged()
        {
            OnPropertyChanged(nameof(HasChanges));
        }

        [RelayCommand]
        private async Task SendToCloud()
        {
            try
            {
                await _restClientService.CreateOrUpdateTodoListAsync(currentList);
                await _dialogService.ShowMopupDialog(EMopUpType.Message, "Erfolg", $"Die Liste - {currentList.Name} - wurde erfolgreich aktualisiert.");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on sending to updated Todo list to Cloud.");
                await _dialogService.ShowMopupDialog(EMopUpType.Message, "Fehler", $"Upps da ging was schief !!! Die Liste - {currentList.Name} - konnte leider nicht in der Cloud aktualisiert werden, Versuche es später nocheinmal.");
            }
        }

        [RelayCommand]
        private async Task AddNewTask()
        {
            IsAddingActive = true;
        }

        [RelayCommand]
        private async Task AddNewTaskEntry()
        {
            try
            {
                var result = "Sonstiges";

                var newTask = new TodoTask
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = NewEntry,
                    IsDone = false,
                    TodoListId = currentList.Id
                };

                
                if (IsShoppingList)
                {
                    result = await _dialogService.ShowCategoriePickerMopupDialog();
                    newTask.CategorieType = CategorieHelper.HandleStringCategorieType(result);
                }
              
                currentList.Todos.Add(newTask);
                await _restClientService.CreateOrUpdateTodoListAsync(currentList);
                await RefreshUpdatedTodoList();
                IsAddingActive = false;
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on adding new Task to Todo list");
            }
        }

        [RelayCommand]
        private async Task DeleteList()
        {
            try
            {
                var res = await _dialogService.ShowMopupDialog(EMopUpType.YesNo, "Achtung !!!", $"Sicher das die Liste - {currentList.Name} - entgültig gelöscht werden soll?");

                if (res)
                {
                    await _restClientService.DeleteTodoListAsync(currentList.Id);
                    await _shellNavigationService.NavigateTo(Constants.ListPage);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on deleting todo list.");
            }
        }

        [RelayCommand]
        private async Task DeleteTask(TodoTask task)
        {
            try
            {
                var res = await _dialogService.ShowMopupDialog(EMopUpType.YesNo, "Achtung !!!", $"Sicher das der Eintrag - {task.Description} - gelöscht werden soll?");

                if (res)
                {
                    await _restClientService.DeleteTodoTaskAsync(task.Id);
                    await RefreshUpdatedTodoList();
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on deleting Task from todo list.");
            }
        }

        private async Task RefreshUpdatedTodoList()
        {
            var updatedList = await _appointmentsTransferService.RefreshCurrentTodoList(currentList.Id);
            this.currentList = updatedList;
            if (IsShoppingList)
            {
                TasksGroups = GroupItems(currentList).ToObservableCollection();
                _originalTasksGroups = CloneList(TasksGroups);
            }
            else
            {
                ListName = currentList.Name;
                Tasks = currentList.Todos.ToObservableCollection();
            }
        }
    }
}
