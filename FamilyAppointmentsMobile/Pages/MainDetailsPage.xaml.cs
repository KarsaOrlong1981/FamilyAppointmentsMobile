using FamilyAppointmentsMobile.ViewModels;

namespace FamilyAppointmentsMobile.Pages;

public partial class MainDetailsPage : ContentPage
{
	public MainDetailsPage()
	{
		InitializeComponent();
		this.BindingContext = new MainDetailsPageViewModel();
	}
}