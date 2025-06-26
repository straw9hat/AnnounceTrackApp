using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnnounceTrackApp.Pages
{
    public enum InterestCategory
    {
        Product,
        Franchise,
        Topic
    }

    public class UserInterest
    {
        public string Name { get; set; }
        public InterestCategory Category { get; set; }
    }

    public partial class InterestsPage : ContentPage
    {
        private List<UserInterest> interests = new();
        public ICommand RemoveCommand { get; }

        private const string InterestsKey = "user_interests";

        public InterestsPage()
        {
            InitializeComponent();

            RemoveCommand = new Command<UserInterest>(OnRemoveInterest);
            BindingContext = this;

            CategoryPicker.ItemsSource = Enum.GetValues(typeof(InterestCategory)).Cast<InterestCategory>().ToList();
            CategoryPicker.SelectedIndex = 0;


            LoadInterests();
        }

        public class InterestGroup : List<UserInterest>
        {
            public InterestCategory Category { get; }

            public InterestGroup(InterestCategory category, IEnumerable<UserInterest> items) : base(items)
            {
                Category = category;
            }
        }

        private void OnAddInterestClicked(object sender, EventArgs e)
        {
            var newName = InterestEntry.Text?.Trim();
            if (string.IsNullOrEmpty(newName)) return;

            var selectedCategory = (InterestCategory)CategoryPicker.SelectedItem;

            if (!interests.Any(i => i.Name.Equals(newName, StringComparison.OrdinalIgnoreCase) && i.Category == selectedCategory))
            {
                interests.Add(new UserInterest
                {
                    Name = newName,
                    Category = selectedCategory
                });

                SaveInterests();
                SortAndRefresh();

                InterestEntry.Text = string.Empty;
                CategoryPicker.SelectedIndex = 0;
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
                ? new List<UserInterest>()
                : JsonSerializer.Deserialize<List<UserInterest>>(json) ?? new List<UserInterest>();

            SortAndRefresh();
        }

        private void SortAndRefresh()
        {
            var grouped = interests
                .OrderBy(i => i.Name)
                .GroupBy(i => i.Category)
                .Select(g => new InterestGroup(g.Key, g))
                .ToList();

            InterestList.ItemsSource = grouped;
        }

        private void OnRemoveInterest(UserInterest interest)
        {
            interests.Remove(interest);
            SaveInterests();
            SortAndRefresh();
        }
    }
}
