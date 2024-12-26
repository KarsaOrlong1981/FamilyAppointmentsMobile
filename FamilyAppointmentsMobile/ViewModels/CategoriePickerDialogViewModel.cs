using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using FamilyAppointmentsMobile.Models;
using FamilyAppointmentsMobile.Services;
using Mopups.Interfaces;


namespace FamilyAppointmentsMobile.ViewModels
{
    public partial class CategoriePickerDialogViewModel : ObservableRecipient
    {
        private readonly IPopupNavigation popupNavigation;
        private readonly IConnectionService connectionService;
        private readonly TaskCompletionSource<string> result;
        private IDialogService dialogService;

        [ObservableProperty] private Categorie pickedItem;
        [ObservableProperty] private List<Categorie> categories;
        public CategoriePickerDialogViewModel(IPopupNavigation popupNavigation, TaskCompletionSource<string> tcs)
        {
            dialogService = Ioc.Default.GetService<IDialogService>();
            connectionService = Ioc.Default.GetService<IConnectionService>();
            this.result = tcs;
            this.popupNavigation = popupNavigation;
            Categories = SetCategories();
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
            result.SetResult("Sonstiges");
            await popupNavigation.PopAsync();
        }

        private List<Categorie> SetCategories()
        {
            return new List<Categorie>
            {
                new Categorie { Name = "Sonstiges" },
                new Categorie { Name = "Brot und Backwaren" },
                new Categorie { Name = "Obst und Gemüse" },
                new Categorie { Name = "Fleisch und Wurstwaren" },
                new Categorie { Name = "Fisch und Meeresfrüchte" },
                new Categorie { Name = "Milchprodukte" },
                new Categorie { Name = "Tiefkühlkost" },
                new Categorie { Name = "Getränke" },
                new Categorie { Name = "Trockenprodukte" },
                new Categorie { Name = "Snacks und Süßigkeiten" },
                new Categorie { Name = "Gewürze und Saucen" },
                new Categorie { Name = "Konserven und Fertiggerichte" },
                new Categorie { Name = "Drogerie" },
                new Categorie { Name = "Spirituosen" },
                new Categorie { Name = "Haushaltwaren" },
                new Categorie { Name = "Tierbedarf" },
                new Categorie { Name = "Schreibwaren" },
                new Categorie { Name = "Elektronik" },
                new Categorie { Name = "Kleidung und Schuhe" },
                new Categorie { Name = "Spielwaren" },
                new Categorie { Name = "Bücher und Zeitschriften" },
                new Categorie { Name = "Blumen und Pflanzen" },
                new Categorie { Name = "Sport und Freizeit" }
            };
        }
    }
}
