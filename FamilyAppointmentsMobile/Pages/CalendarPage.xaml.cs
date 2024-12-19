using FamilyAppointmentsMobile.ViewModels;

namespace FamilyAppointmentsMobile.Pages;

public partial class CalendarPage : ContentPage
{
	public CalendarPage()
	{
		InitializeComponent();
		this.BindingContext = new CalendarPageViewModel();
	}

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
		calendar.Dispose();
	}
}