// <copyright file="AddFriendsViewModel.cs" company="PlaceholderCompany">
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

    public class AddFriendsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> UsersList { get; set; }

        private FriendsListViewModel friendsListViewModel;

        private string searchQuery;

        public List<User> allUsers { get; set; }

        public IUserService userService { get; set; }

        public ICommand AddFriendCommand { get; set; }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterUsers();
                }
            }
        }

        public AddFriendsViewModel(FriendsListViewModel friendsListViewModel, IUserService userService)
        {
            this.userService = userService;
            this.friendsListViewModel = friendsListViewModel;
            this.allUsers = userService.GetNonFriendsUsers(userService.GetCurrentUser());
            this.UsersList = new ObservableCollection<User>();
            this.AddFriendCommand = new RelayCommand<object>(AddFriend);

            this.LoadUsers();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddFriend(object user)
        {
            var friend = user as User;

            this.userService.AddFriend(this.userService.GetCurrentUser(), friend!.GetUserId());
            this.friendsListViewModel.LoadFriends();
            this.LoadUsers();
        }

        private void LoadUsers()
        {
            this.allUsers = this.userService.GetNonFriendsUsers(this.userService.GetCurrentUser());
            this.FilterUsers();
        }

        private void FilterUsers()
        {
            this.UsersList.Clear();

            foreach (var friend in this.allUsers.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.UsersList.Add(friend);
            }
        }
    }
}
