using Microsoft.UI.Xaml.Controls;
using SocialStuff.Model;
using SocialStuff.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialStuff.ViewModel
{
    public class CreateChatViewModel : INotifyPropertyChanged
    {
        private string groupName;
        private string searchQuery;
        private UserService userService;
        private ChatService chatService;
        private ChatListViewModel chatListViewModel;
        public ICommand AddToSelectedList { get; }
        public ICommand CreateGroupChat { get; }

        public ObservableCollection<User> Friends { get; set; }
        private List<User> allFriends;
        public ObservableCollection<User> SelectedFriends { get; set; }

        public string GroupName
        {
            get => groupName;
            set
            {
                if (groupName != value)
                {
                    groupName = value;
                    OnPropertyChanged(nameof(GroupName));
                }
            }
        }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged(nameof(searchQuery));
                    FilterFriends();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateChatViewModel(ChatListViewModel chatListViewModel, ChatService chatService, UserService userService)
        {
            this.chatListViewModel = chatListViewModel;
            AddToSelectedList = new RelayCommand<object>(AddFriendToSelectedList);
            CreateGroupChat = new RelayCommand(AddNewGroupChat);
            Friends = new ObservableCollection<User>();
            SelectedFriends = new ObservableCollection<User>();
            this.chatService = chatService;
            this.userService = userService;
            allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());

            LoadFriends();
        }

        private void AddNewGroupChat()
        {
            List<int> selectedFriendsIDs = new List<int>();
            selectedFriendsIDs.Add(userService.GetCurrentUser());
            foreach (User friend in SelectedFriends)
            {
                selectedFriendsIDs.Add(friend.GetUserId());
            }
            chatService.createChat(selectedFriendsIDs, GroupName);
            this.chatListViewModel.LoadChats();
        }

        private void AddFriendToSelectedList(object parameter)
        {
            var friend = parameter as User;
            if (friend != null && !SelectedFriends.Contains(friend))
            {
                SelectedFriends.Add(friend);
                FilterFriends();

            }
        }

        private void LoadFriends()
        {
            FilterFriends();
        }

        private void FilterFriends()
        {
            Friends.Clear();

            foreach (var friend in allFriends.Where(f =>
                         string.IsNullOrEmpty(SearchQuery) ||
                         f.Username.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                Friends.Add(friend);
            }

            foreach(var friend in SelectedFriends)
            {
                Friends.Remove(friend);
            }
        }
    }
}
