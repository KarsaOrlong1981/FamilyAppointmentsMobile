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
    public partial class ListPageViewModel : ObservableRecipient
    {
        private readonly ILogger<ListPageViewModel> log;
        private readonly IDialogService _dialogService;
        private readonly IShellNavigationService _shellNavigationService;
        private readonly IRestClientService _restClientService;
        private readonly IConnectionService _connectionService;
        private readonly IAppointmentsTransferService _appointmentsTransferService;
        [ObservableProperty] private TodoList selectedList;
        [ObservableProperty] private ObservableCollection<TodoList> taskLists;

        public ListPageViewModel()
        {
            log = Ioc.Default.GetService<ILogger<ListPageViewModel>>();
            _dialogService = Ioc.Default.GetService<IDialogService>();
            _shellNavigationService = Ioc.Default.GetService<IShellNavigationService>();
            _restClientService = Ioc.Default.GetService<IRestClientService>();
            _connectionService = Ioc.Default.GetService<IConnectionService>();
            _appointmentsTransferService = Ioc.Default.GetService<IAppointmentsTransferService>();
            _restClientService.TaskListsChanged += _restClientService_TaskListsChanged;
            _restClientService.TodoChangedNotificationReceived += _restClientService_TodoChangedNotificationReceived;
            TaskLists = new ObservableCollection<TodoList>();
            LoadTodos();
        }

        private void _restClientService_TodoChangedNotificationReceived(object sender, EventArgs e)
        {
            LoadTodos();
        }

        private void _restClientService_TaskListsChanged(object sender, TaskListsChangedEventArgs e)
        {
            try
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    switch(e.TodoOperationType)
                    {
                        case ETodoOperationType.AddList: TaskLists.Add(e.TodoList); break;
                        case ETodoOperationType.RemoveList: TaskLists.Remove(e.TodoList); break;
                    }
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on Todo lists changed");
            }
        }

        private void LoadTodos()
        {
            try
            {
                DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                {
                    await _appointmentsTransferService.LoadTodos();
                    var taskList = _appointmentsTransferService.TodoLists;
                    TaskLists = taskList.ToObservableCollection();
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error on loading Todo lists from cloud");
            } 
        }

        [RelayCommand]
        private async Task AddList()
        {
            await _dialogService.ShowMopupDialog(EMopUpType.AddList);
        }

        [RelayCommand]
        private async Task SelectedListAction()
        {
            _appointmentsTransferService.CurrentTodoList = SelectedList;
            await _shellNavigationService.NavigateTo(Constants.ListDetailsPage);
            _shellNavigationService.OnNavigationLayoutChanged(SelectedList.Name);
        }
    }
}
