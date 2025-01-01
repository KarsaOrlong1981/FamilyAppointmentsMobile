using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Helpers;
using FamilyAppointmentsMobile.Models;
using Mopups.Interfaces;


namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class CategoriePickerDialogViewModel : ObservableRecipient
    {
        private readonly IPopupNavigation popupNavigation;
        private readonly TaskCompletionSource<string> result;

        [ObservableProperty] private Categorie pickedItem;
        [ObservableProperty] private List<Categorie> categories;
        public CategoriePickerDialogViewModel(IPopupNavigation popupNavigation, TaskCompletionSource<string> tcs)
        {
            this.result = tcs;
            this.popupNavigation = popupNavigation;
            Categories = CategorieHelper.SetCategories();
        }

        [RelayCommand]
        private async Task PickedItemChanged()
        {
            if (!string.IsNullOrEmpty(PickedItem.Name))
            {
                result.SetResult(PickedItem.Name);
                await popupNavigation.PopAsync();
            }    
        }

        [RelayCommand]
        private async Task CloseDialog()
        {
            result.SetResult(Constants.Sonstiges);
            await popupNavigation.PopAsync();
        }
    }
}
