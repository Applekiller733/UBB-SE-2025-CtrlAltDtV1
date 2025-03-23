using Microsoft.UI.Xaml.Controls;
using SocialStuff.ViewModel;
using SocialStuff.Services;
using SocialStuff.Data;

namespace SocialStuff.View
{
    public sealed partial class FeedView : Page
    {
        private FeedViewModel feedViewModel;
        private UserService userService;
        private FeedService feedService;
        public FeedView(FeedViewModel feedViewModel, UserService userService, FeedService feedService)
        {
            this.InitializeComponent();
            this.userService = userService;
            this.feedService = feedService;
            this.feedViewModel = feedViewModel;
            this.DataContext = feedViewModel;
        }
    }
}