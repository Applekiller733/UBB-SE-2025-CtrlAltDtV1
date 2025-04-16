// <copyright file="FeedView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    public sealed partial class FeedView : Page
    {
        private FeedViewModel feedViewModel;
        private IUserService userService;
        private IFeedService feedService;

        public FeedView(FeedViewModel feedViewModel, IUserService userService, IFeedService feedService)
        {
            this.InitializeComponent();
            this.userService = userService;
            this.feedService = feedService;
            this.feedViewModel = feedViewModel;
            this.DataContext = feedViewModel;
        }
    }
}
