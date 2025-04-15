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
using SocialStuff.Services.Implementations;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff.View
{

    public sealed partial class LeaveChatView : Page
    {
        private ChatService chatService;
        private UserService userService;
        private Frame RightFrame;
        private LeaveChatViewModel leaveChatViewModel;
        private Page lastPage;
        private ChatListViewModel chatMessagesViewModel;

        public LeaveChatView(int ChatID, ChatListViewModel chVm, Page chatM, Frame right, ChatService chat, UserService user)
        {
            this.InitializeComponent();
            this.chatMessagesViewModel = chVm;
            this.lastPage = chatM;
            this.RightFrame = right;
            this.userService = user;
            this.chatService = chat;

            leaveChatViewModel = new LeaveChatViewModel(userService, chatService, chatMessagesViewModel, ChatID);
            this.DataContext = leaveChatViewModel;
        }

        public void LeaveChat_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = null;
        }

        public void CancelLeaving_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = lastPage;
        }
    }
}
