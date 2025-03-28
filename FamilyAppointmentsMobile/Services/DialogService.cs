﻿using CommunityToolkit.Mvvm.DependencyInjection;
using FamilyAppointmentsMobile.Helpers;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.ViewModels;
using FamilyAppointmentsMobile.Dialogs;
using Mopups.Interfaces;
using Mopups.Pages;

namespace FamilyAppointmentsMobile.Services
{
    public class DialogService : IDialogService
    {
        private string title;
        private string message;
        private string member;
        private Appointment appointment;
        private TodoList todos;
        private IPopupNavigation popupNavigation;

        public DialogService()
        {
            popupNavigation = Ioc.Default.GetService<IPopupNavigation>();
        }

        public Task<bool> ShowMopupDialog(EMopUpType mopUpType, string title = "", string message = "", string member = "", Appointment appointment = null, bool isPendingItem = false, TodoList todos = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var pickerTcs = new TaskCompletionSource<string>();
            this.title = title;
            this.message = message;
            this.member = member;
            this.appointment = appointment;
            this.todos = todos;

            switch (mopUpType)
            {
                case EMopUpType.Message: OpenMessageMopUp(tcs); break;
                case EMopUpType.YesNo: OpenYesNoMopUp(tcs); break;
                case EMopUpType.Add: OpenAddMopUp(tcs); break;
                case EMopUpType.Edit: OpenEditMopUp(tcs, isPendingItem); break;
                case EMopUpType.AddList: OpenAddListMopUp(tcs); break;
                case EMopUpType.EditList: OpenEditListMopUp(tcs); break;
                case EMopUpType.PickCategorie: OpenCategoriePickerMopUp(pickerTcs); break;
            }
            
            return tcs.Task;
        }

        public Task<string> ShowCategoriePickerMopupDialog()
        {
            var pickerTcs = new TaskCompletionSource<string>();
            OpenCategoriePickerMopUp(pickerTcs); 
            return pickerTcs.Task;
        }

        private void OpenEditMopUp(TaskCompletionSource<bool> tcs, bool isPendingItem = false)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs, EMopUpType.Edit, appointment: appointment, isPendingItem: isPendingItem);
                var dialog = new MopUpEditDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenAddMopUp(TaskCompletionSource<bool> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs, EMopUpType.Add, member: member);
                var dialog = new MopUpAddDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenCategoriePickerMopUp(TaskCompletionSource<string> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new CategoriePickerDialogViewModel(popupNavigation, tcs);
                var dialog = new CategoriePickerDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenAddListMopUp(TaskCompletionSource<bool> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs, EMopUpType.AddList);
                var dialog = new MopUpAddListDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenEditListMopUp(TaskCompletionSource<bool> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs, EMopUpType.EditList, todos: this.todos);
                var dialog = new MopUpEditListsDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenYesNoMopUp(TaskCompletionSource<bool> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs, EMopUpType.YesNo, title, message);
                var dialog = new MopUpMessageDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync(dialog);
            }));
        }

        private void OpenMessageMopUp(TaskCompletionSource<bool> tcs)
        {
            DispatcherHelper.CheckBeginInvokeOnUI((Action)(async () =>
            {
                var vm = new MopUpDialogViewModel(popupNavigation, tcs , EMopUpType.Message, title, message);
                var dialog = new MopUpMessageDialog();
                dialog.BindingContext = vm;
                await popupNavigation.PushAsync((PopupPage)dialog);
            }));
        }
    }
}
