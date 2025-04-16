// <copyright file="AddNewMemberView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    public sealed partial class AddNewMemberView : Page
    {
        private IChatService chatService;
        private IUserService userService;
        private AddNewMemberViewModel addNewMemberViewModel;
        private Page lastChat;
        private Frame RightFrame;
        private ChatMessagesViewModel chatMessagesViewModel;

        public AddNewMemberView(ChatMessagesViewModel chatMessagesViewModel, Page lastChat, Frame rightFrame, int ChatID, IChatService chatService, IUserService userService)
        {
            this.InitializeComponent();
            this.chatMessagesViewModel = chatMessagesViewModel;
            this.lastChat = lastChat;
            this.RightFrame = rightFrame;
            this.chatService = chatService;
            this.userService = userService;
            this.addNewMemberViewModel = new AddNewMemberViewModel(chatMessagesViewModel, lastChat, ChatID, chatService, userService);

            this.DataContext = this.addNewMemberViewModel;
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = this.lastChat;
        }
    }
}
