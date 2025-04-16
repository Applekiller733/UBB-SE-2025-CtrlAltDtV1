// <copyright file="CreateChatViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class CreateChatViewModel : INotifyPropertyChanged
    {
        private string groupName;
        private string searchQuery;
        private IUserService userService;
        private IChatService chatService;
        private ChatListViewModel chatListViewModel;

        public ICommand AddToSelectedList { get; }

        public ICommand CreateGroupChat { get; }

        public ObservableCollection<User> Friends { get; set; }

        private List<User> allFriends;

        public ObservableCollection<User> SelectedFriends { get; set; }

        public string GroupName
        {
            get => this.groupName;
            set
            {
                if (this.groupName != value)
                {
                    this.groupName = value;
                    this.OnPropertyChanged(nameof(this.GroupName));
                }
            }
        }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.searchQuery));
                    this.FilterFriends();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateChatViewModel(ChatListViewModel chatListViewModel, IChatService chatService, IUserService userService)
        {
            this.chatListViewModel = chatListViewModel;
            this.AddToSelectedList = new RelayCommand<object>(this.AddFriendToSelectedList);
            this.CreateGroupChat = new RelayCommand(this.AddNewGroupChat);
            this.Friends = new ObservableCollection<User>();
            this.SelectedFriends = new ObservableCollection<User>();
            this.chatService = chatService;
            this.userService = userService;
            this.allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());

            this.LoadFriends();
        }

        private void AddNewGroupChat()
        {
            List<int> selectedFriendsIDs = new List<int>();
            selectedFriendsIDs.Add(this.userService.GetCurrentUser());
            foreach (User friend in this.SelectedFriends)
            {
                selectedFriendsIDs.Add(friend.GetUserId());
            }

            this.chatService.CreateChat(selectedFriendsIDs, this.GroupName);
            this.chatListViewModel.LoadChats();
        }

        private void AddFriendToSelectedList(object parameter)
        {
            var friend = parameter as User;
            if (friend != null && !this.SelectedFriends.Contains(friend))
            {
                this.SelectedFriends.Add(friend);
                this.FilterFriends();
            }
        }

        private void LoadFriends()
        {
            this.FilterFriends();
        }

        private void FilterFriends()
        {
            this.Friends.Clear();

            foreach (var friend in this.allFriends.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.Friends.Add(friend);
            }

            foreach (var friend in this.SelectedFriends)
            {
                this.Friends.Remove(friend);
            }
        }
    }
}
