using Microsoft.Maui.Controls;
using System.Diagnostics;
using Microsoft.Maui.Authentication;

namespace AnnounceTrackApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnGoogleSignInClicked(object sender, EventArgs e)
    {
        try
        {
            var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new Uri("https://accounts.google.com/o/oauth2/v2/auth?" +
                        "client_id=835284169131-7br7p2df486hh6oov03u0akb619cg069&" +
                        "redirect_uri=com.googleusercontent.apps.835284169131-7br7p2df486hh6oov03u0akb619cg069:/oauth2redirect&" +
                        "response_type=code&" +
                        "scope=openid%20email%20profile"),
                new Uri("com.googleusercontent.apps.835284169131-7br7p2df486hh6oov03u0akb619cg069:/oauth2redirect"));

            var code = authResult?.Properties["code"];
            if (code != null)
            {
                await ExchangeCodeForIdToken(code);
                await DisplayAlert("Success", "Signed in successfully!", "OK");
            }

            // After token exchange and successful login
            await DisplayAlert("Success", "Signed in successfully!", "OK");

            // Navigate to home page
            await Navigation.PushAsync(new HomePage());

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Sign-in failed: {ex.Message}");
            await DisplayAlert("Error", "Login failed. Try again.", "OK");
        }
    }

    private async Task ExchangeCodeForIdToken(string code)
    {
        using var client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("client_id", "835284169131-7br7p2df486hh6oov03u0akb619cg069"),
            new KeyValuePair<string, string>("client_secret", "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"),
            new KeyValuePair<string, string>("redirect_uri", "com.googleusercontent.apps.835284169131-7br7p2df486hh6oov03u0akb619cg069:/oauth2redirect"),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
        });

        var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
        var result = await response.Content.ReadAsStringAsync();
        Debug.WriteLine("Token exchange result: " + result);
    }
}
