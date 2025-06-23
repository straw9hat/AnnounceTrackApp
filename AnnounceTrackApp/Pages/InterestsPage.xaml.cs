using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnnounceTrackApp.Pages
{
    public partial class InterestsPage : ContentPage
    {
        private List<string> interests = new();
        public ICommand RemoveCommand { get; }

        private const string InterestsKey = "user_interests";

        public InterestsPage()
        {
            InitializeComponent();
            RemoveCommand = new Command<string>(OnRemoveInterest);
            BindingContext = this;
            LoadInterests();
        }

        private void OnAddInterestClicked(object sender, EventArgs e)
        {
            var newInterest = InterestEntry.Text?.Trim();

            if (!string.IsNullOrEmpty(newInterest) && !interests.Contains(newInterest))
            {
                interests.Add(newInterest);
                SaveInterests(); 

                InterestList.ItemsSource = null;
                InterestList.ItemsSource = interests;

                InterestEntry.Text = string.Empty;
            }
        }

        private void SaveInterests()
        {
            var json = JsonSerializer.Serialize(interests);
            Preferences.Set(InterestsKey, json);
        }

        private void LoadInterests()
        {
            var json = Preferences.Get(InterestsKey, string.Empty);
            interests = string.IsNullOrEmpty(json)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

            InterestList.ItemsSource = interests;
        }

        private void OnRemoveInterest(string interest)
        {
            if (interests.Contains(interest))
            {
                interests.Remove(interest);
                SaveInterests();

                InterestList.ItemsSource = null;
                InterestList.ItemsSource = interests;
            }
        }
    }
}
