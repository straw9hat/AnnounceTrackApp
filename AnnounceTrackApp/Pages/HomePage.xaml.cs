namespace AnnounceTrackApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
    }
    
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Optional: Clear any saved tokens here

        // Navigate back to LoginPage
        await Navigation.PopToRootAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        // Do nothing to prevent going back
        return true;
    }

    private async void OnViewInterestsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InterestsPage());
    }
}
