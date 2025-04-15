using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SocialStuff.ViewModel;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff.View
{
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
            chatListViewModel = new ChatListViewModel(chatService, userService);
            MainGrid.DataContext = chatListViewModel;
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = new CreateChatView(chatListViewModel, chatService, userService);
        }


        private void ChatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChatList.SelectedItem is Chat selectedChat)
            {
                this.RightFrame.Content = new ChatMessagesView(chatListViewModel, mainWindow, RightFrame, selectedChat.getChatID(), userService, chatService, messageService, reportService);
            }
        }

    }
}
