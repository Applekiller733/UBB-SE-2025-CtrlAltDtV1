// <copyright file="FriendsListView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FriendsListView : Page
    {
        private FriendsListViewModel friendsListViewModel;
        private IChatService chatService;
        private IUserService userService;
        private IMessageService messageService;
        private Frame RightFrame;
        private Page AddFriendsPage;

        public FriendsListView(IChatService chatService, IUserService userService, IMessageService messageService, Frame RightFrame)
        {
            this.InitializeComponent();
            this.chatService = chatService;
            this.userService = userService;
            this.messageService = messageService;
            this.RightFrame = RightFrame;
            this.friendsListViewModel = new FriendsListViewModel(chatService, userService, messageService);

            this.DataContext = this.friendsListViewModel;
        }

        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new AddFriendsView(this.friendsListViewModel, this.userService);
        }
    }
}