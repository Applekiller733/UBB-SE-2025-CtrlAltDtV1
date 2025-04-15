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
    public sealed partial class CreateChatView : Page
    {
        public ChatService chatService;
        public UserService userService;

        public CreateChatView(ChatListViewModel chatListViewModel, ChatService chatService, UserService userService)
        {
            this.InitializeComponent();
            this.chatService = chatService;
            this.userService = userService;

            MainGrid.DataContext = new CreateChatViewModel(chatListViewModel, chatService, userService);
        }
    }
}
