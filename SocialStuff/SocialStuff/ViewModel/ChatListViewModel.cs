using SocialStuff.View;
using SocialStuff.Model;
using SocialStuff.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;

namespace SocialStuff.ViewModel
{
    public class ChatListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string searchQuery = string.Empty;
        public ObservableCollection<Chat> ChatList { get; set; }
        public List<Chat> currentUserChats;
        public ChatService chatService;
        public UserService userService;
        public CountToVisibilityConverter CountToVisibilityConverter { get; set; }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));
                    FilterChats();
                }
            }
        }

        public ChatListViewModel(ChatService chatS, UserService userS)
        {
            ChatList = new ObservableCollection<Chat>();
            chatService = chatS;
            userService = userS;
            currentUserChats = this.userService.GetCurrentUserChats();
            CountToVisibilityConverter = new CountToVisibilityConverter();

            LoadChats();
        }

        public void LoadChats()
        {
            FilterChats();
        }

        public void FilterChats()
        {
            ChatList.Clear();
            currentUserChats = this.userService.GetCurrentUserChats();
            foreach (var chat in currentUserChats)
            {
                if (string.IsNullOrEmpty(SearchQuery) ||
                    chat.getChatName().IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ChatList.Add(chat);
                }
            }
            // sort chats by last message time
            ChatList = new ObservableCollection<Chat>(ChatList.OrderByDescending(chat => chatService.getLastMessageTimeStamp(chat.getChatID())));

        }
    }
    
}
