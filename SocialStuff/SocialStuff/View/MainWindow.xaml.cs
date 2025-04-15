using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SocialStuff.Data.Database;
using SocialStuff.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SocialStuff.View;
using SocialStuff.ViewModel;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff
{
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

            mainWindow = this;
            IRepository repo = new Repository();
            notificationService = new NotificationService(repo);
            userService = new UserService(repo, notificationService);
            chatService = new ChatService(repo);
            messageService = new MessageService(repo, userService);
            feedService = new FeedService(repo, userService);
            reportService = new ReportService(repo, userService);

            if (LeftFrame.Content == null || !(LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, chatService, userService, reportService, messageService, this.RightFrame);
                LeftFrame.Content = chatListView;
            }
            if (RightFrame.Content == null || !(RightFrame.Content is FeedView))
            {

                var feedViewModel = new FeedViewModel(feedService);
                var feedView = new FeedView(feedViewModel, userService, feedService);
                RightFrame.Content = feedView;
            }
        }

        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            if (LeftFrame.Content == null || !(LeftFrame.Content is ChatListView))
            {
                var chatListView = new ChatListView(this, chatService, userService, reportService, messageService, this.RightFrame);
                LeftFrame.Content = chatListView;
            }
        }

        //private void Feed_Click(object sender, RoutedEventArgs e)
        //{
        //    RightFrame.Navigate(typeof(FeedView));
        //}

        private void Friends_Click(object sender, RoutedEventArgs e)
        {
            if (LeftFrame.Content == null || !(LeftFrame.Content is FriendsListView))
            {
                var friendsListView = new FriendsListView(chatService, userService, messageService, this.RightFrame);
                LeftFrame.Content = friendsListView;

            }
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (RightFrame.Content == null || !(RightFrame.Content is FeedView))
            {

                var feedViewModel = new FeedViewModel(feedService);
                var feedView = new FeedView(feedViewModel, userService, feedService);
                RightFrame.Content = feedView;
            }
        }

        private void Notifications_click(object sender, RoutedEventArgs e)
        {
            if (RightFrame.Content == null || !(RightFrame.Content is NotificationView))
            {
                var notificationView = new NotificationView();
                RightFrame.Content = notificationView;
            }
        }
    }

}
