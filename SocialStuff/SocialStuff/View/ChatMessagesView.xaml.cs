// <copyright file="ChatMessagesView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    public sealed partial class ChatMessagesView : Page
    {
        public int SelectedChat { get; set; }

        private ChatMessagesViewModel chatMessagesViewModel;
        private Frame RightFrame;
        private IUserService userService;
        private IChatService chatService;
        private IReportService reportService;
        private ChatListViewModel chatListViewModel;
        private GenerateTransferViewModel GenerateTransferViewModel;

        public ChatMessagesView(ChatListViewModel chatListViewModel, Window mainWindow, Frame RightFrame, int ChatID, IUserService userService, IChatService chatService, IMessageService messageService, IReportService reportService)
        {
            this.InitializeComponent();
            this.SelectedChat = ChatID;
            this.chatListViewModel = chatListViewModel;
            this.userService = userService;
            this.chatService = chatService;
            this.reportService = reportService;
            this.RightFrame = RightFrame;
            this.chatMessagesViewModel = new ChatMessagesViewModel(mainWindow,RightFrame, ChatID, messageService, chatService, userService, reportService);
            this.GenerateTransferViewModel = new GenerateTransferViewModel(chatService, ChatID);
            this.chatMessagesViewModel.ChatListView = this.ChatListView;
            this.chatMessagesViewModel.SetupMessageTracking();

            this.DataContext = this.chatMessagesViewModel;
        }

        public void AddNewMember_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new AddNewMemberView(this.chatMessagesViewModel, this, this.RightFrame, this.SelectedChat, this.chatService, this.userService);
        }

        public void LeaveChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new LeaveChatView(this.SelectedChat, this.chatListViewModel, this, this.RightFrame, this.chatService, this.userService);
        }

        public void SendTransfer_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new GenerateTransferView(this.GenerateTransferViewModel, this, this.RightFrame, this.SelectedChat, this.chatService);
        }
    }
}