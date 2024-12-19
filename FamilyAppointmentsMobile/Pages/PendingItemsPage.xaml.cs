using FamilyAppointmentsMobile.ViewModels;

namespace FamilyAppointmentsMobile.Pages;

public partial class PendingItemsPage : ContentPage
{
	public PendingItemsPage()
	{
		InitializeComponent();
		this.BindingContext = new PendingItemsViewModel();
	}
}