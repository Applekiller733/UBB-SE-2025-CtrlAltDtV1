// <copyright file="FriendsListViewModel.cs" company="PlaceholderCompany">
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
    using Microsoft.UI.Xaml;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class FriendsListViewModel : INotifyPropertyChanged
    {
        public List<User> allFriends { get; set; }

        public ObservableCollection<User> FriendsList { get; set; }

        public IUserService userService { get; set; }

        public IChatService chatService { get; set; }

        public IMessageService messageService { get; set; }

        public ICommand RemoveFriend { get; }

        private Visibility _noFriendsVisibility = Visibility.Collapsed;

        public Visibility NoFriendsVisibility
        {
            get
            {
                return this._noFriendsVisibility;
            }

            set
            {
                this._noFriendsVisibility = value;
                this.OnPropertyChanged(nameof(this.NoFriendsVisibility));
            }
        }

        private string searchQuery;

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterFriends();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FriendsListViewModel(IChatService chat, IUserService user, IMessageService message)
        {
            this.userService = user;
            this.chatService = chat;
            this.messageService = message;
            this.allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());
            this.FriendsList = new ObservableCollection<User>();
            this.RemoveFriend = new RelayCommand<object>(this.RemoveFriendFromList);

            this.LoadFriends();
        }

        public void RemoveFriendFromList(object user)
        {
            var friend = user as User;
            if (friend != null)
            {
                this.userService.RemoveFriend(this.userService.GetCurrentUser(), friend.GetUserId());
            }

            this.allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());

            this.LoadFriends();
        }

        public void LoadFriends()
        {
            this.allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());
            this.FilterFriends();
        }

        public void FilterFriends()
        {
            this.FriendsList.Clear();

            foreach (var friend in this.allFriends.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                         f.PhoneNumber.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.FriendsList.Add(friend);
            }

            this.UpdateNoFriendsVisibility();
        }

        private void UpdateNoFriendsVisibility()
        {
            this.NoFriendsVisibility = (this.FriendsList == null || this.FriendsList.Count == 0)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}