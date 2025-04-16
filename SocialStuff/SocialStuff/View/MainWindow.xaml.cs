// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff
{
    using Microsoft.UI.Xaml;
    using SocialStuff.Data;
    using SocialStuff.Services.Implementations;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.View;
    using SocialStuff.ViewModel;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Window mainWindow;
        private IUserService userService;
        private IChatService chatService;
        private IMessageService messageService;
        private IFeedService feedService;
        private IReportService reportService;
        private INotificationService notificationService;

        public MainWindow()
        {
            this.InitializeComponent();

            this.mainWindow = this;
            IRepository repo = new Repository();
            this.notificationService = new NotificationService(repo);
            this.userService = new UserService(repo, this.notificationService);
            this.chatService = new ChatService(repo);
            this.messageService = new MessageService(repo, this.userService);
            this.feedService = new FeedService(repo, this.userService);
            this.reportService = new ReportService(repo, this.userService);

            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, this.chatService, this.userService, this.reportService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = chatListView;
            }

            if (this.RightFrame.Content == null || !(this.RightFrame.Content is FeedView))
            {
                var feedViewModel = new FeedViewModel(this.feedService);
                var feedView = new FeedView(feedViewModel, this.userService, this.feedService);
                this.RightFrame.Content = feedView;
            }
        }

        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, this.chatService, this.userService, this.reportService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = chatListView;
            }
        }

        // private void Feed_Click(object sender, RoutedEventArgs e)
        // {
        //    RightFrame.Navigate(typeof(FeedView));
        // }
        private void Friends_Click(object sender, RoutedEventArgs e)
        {
            if (this.LeftFrame.Content == null || !(this.LeftFrame.Content is FriendsListView))
            {
                var friendsListView = new FriendsListView(this.chatService, this.userService, this.messageService, this.RightFrame);
                this.LeftFrame.Content = friendsListView;
            }
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (this.RightFrame.Content == null || !(this.RightFrame.Content is FeedView))
            {

                var feedViewModel = new FeedViewModel(this.feedService);
                var feedView = new FeedView(feedViewModel, this.userService, this.feedService);
                this.RightFrame.Content = feedView;
            }
        }

        private void Notifications_click(object sender, RoutedEventArgs e)
        {
            if (this.RightFrame.Content == null || !(this.RightFrame.Content is NotificationView))
            {
                var notificationView = new NotificationView();
                this.RightFrame.Content = notificationView;
            }
        }
    }
}
