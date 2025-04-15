using Microsoft.UI.Xaml.Controls;
using SocialStuff.ViewModel;
using SocialStuff.Data;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;

namespace SocialStuff.View
{
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
