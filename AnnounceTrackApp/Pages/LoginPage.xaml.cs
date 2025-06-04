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
                        "client_id=05f717aa9fcbc3fffb3072&" +
                        "redirect_uri=com.googleusercontent.apps.05f717aa9fcbc3fffb3072:/oauth2redirect&" +
                        "response_type=code&" +
                        "scope=openid%20email%20profile"),
                new Uri("com.googleusercontent.apps.05f717aa9fcbc3fffb3072:/oauth2redirect"));

            var code = authResult?.Properties["code"];
            if (code != null)
            {
                await ExchangeCodeForIdToken(code);
                await DisplayAlert("Success", "Signed in successfully!", "OK");
            }
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
            new KeyValuePair<string, string>("client_id", "05f717aa9fcbc3fffb3072"),
            new KeyValuePair<string, string>("client_secret", "584403066487"),
            new KeyValuePair<string, string>("redirect_uri", "com.googleusercontent.apps.YOUR_WEB_CLIENT_ID:/oauth2redirect"),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
        });

        var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
        var result = await response.Content.ReadAsStringAsync();
        Debug.WriteLine("Token exchange result: " + result);
    }
}
