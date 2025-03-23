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
using SocialStuff.Services;
using SocialStuff.ViewModel;
using SocialStuff.Model;

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
        private UserService userService;
        private ChatService chatService;
        private MessageService messageService;

        public MainWindow()
        {
            this.InitializeComponent();

            mainWindow = this;
            Repository repo = new Repository();
            userService = new UserService(repo);
            chatService = new ChatService(repo);
            messageService = new MessageService(repo);
            
            ChatListViewModel chatListViewModel = new ChatListViewModel(chatService, userService);

            MainGrid.DataContext = chatListViewModel;
        }

        private void ChatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChatList.SelectedItem is Chat selectedChat)
            {
                this.DispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        var chatMessagesView = new ChatMessagesView(
                            selectedChat.getChatID(),
                            userService,
                            chatService,
                            messageService);

                        ContentFrame.Content = chatMessagesView;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error setting content: {ex.Message}");
                    }
                });
            }
        }
    }

    public class NavigationParameters
    {
        public int ChatID { get; set; }
        public UserService UserService { get; set; }
        public ChatService ChatService { get; set; }
        public MessageService MessageService { get; set; }
    }
}
