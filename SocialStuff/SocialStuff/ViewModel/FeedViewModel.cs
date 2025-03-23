using SocialStuff.Model;
using SocialStuff.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SocialStuff.ViewModel
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        private readonly FeedService feedService;
        private ObservableCollection<Post> posts;

        public ObservableCollection<Post> Posts
        {
            get { return posts; }
            set
            {
                posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public FeedViewModel()
        {
            // Default constructor for XAML
        }

        public FeedViewModel(FeedService service)
        {
            this.feedService = service;
            LoadPosts();
        }

        public void LoadPosts()
        {
            var feedContent = feedService.GetFeedContent();
            Posts = new ObservableCollection<Post>(feedContent);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}