// <copyright file="ChatListView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatListView : Page
    {
        private ChatListViewModel chatListViewModel;
        public IUserService userService;
        public IChatService chatService;
        public IMessageService messageService;
        public IReportService reportService;
        public Frame RightFrame;
        public Window mainWindow;

        public ChatListView(Window mainWindow, IChatService chatService, IUserService userService, IReportService reportService, IMessageService messageService, Frame RightFrame)
        {
            this.InitializeComponent();

            this.mainWindow = mainWindow;
            this.userService = userService;
            this.chatService = chatService;
            this.messageService = messageService;
            this.reportService = reportService;
            this.RightFrame = RightFrame;
            this.chatListViewModel = new ChatListViewModel(chatService, userService);
            this.MainGrid.DataContext = this.chatListViewModel;
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new CreateChatView(this.chatListViewModel, this.chatService, this.userService);
        }

        private void ChatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ChatList.SelectedItem is Chat selectedChat)
            {
                this.RightFrame.Content = new ChatMessagesView(this.chatListViewModel, this.mainWindow, this.RightFrame, selectedChat.getChatID(), this.userService, this.chatService, this.messageService, this.reportService);
            }
        }
    }
}
