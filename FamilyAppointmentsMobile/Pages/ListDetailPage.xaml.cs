using FamilyAppointmentsMobile.ViewModels;

namespace FamilyAppointmentsMobile.Pages;

public partial class ListDetailPage : ContentPage
{
	public ListDetailPage()
	{
		InitializeComponent();
		this.BindingContext = new ListDetailsPageViewModel();
	}
}