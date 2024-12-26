using FamilyAppointmentsMobile.ViewModels;

namespace FamilyAppointmentsMobile.Pages;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
		this.BindingContext = new ListPageViewModel();
	}
}