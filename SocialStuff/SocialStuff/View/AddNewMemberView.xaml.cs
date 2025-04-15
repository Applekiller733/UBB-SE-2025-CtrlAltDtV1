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
using SocialStuff.Services.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff.View
{
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

            this.DataContext = addNewMemberViewModel;
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.RightFrame.Content = lastChat;
        }
    }
}
