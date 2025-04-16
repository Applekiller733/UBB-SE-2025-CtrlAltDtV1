// <copyright file="FeedViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class FeedViewModel : INotifyPropertyChanged
    {
        private readonly IFeedService feedService;
        private ObservableCollection<Post> posts;

        public ObservableCollection<Post> Posts
        {
            get
            {
                return this.posts;
            }

            set
            {
                this.posts = value;
                this.OnPropertyChanged(nameof(this.Posts));
            }
        }

        public FeedViewModel()
        {
            // Default constructor for XAML
        }

        public FeedViewModel(IFeedService service)
        {
            this.feedService = service;
            this.LoadPosts();
        }

        public void LoadPosts()
        {
            var feedContent = this.feedService.GetFeedContent();
            this.Posts = new ObservableCollection<Post>(feedContent);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}