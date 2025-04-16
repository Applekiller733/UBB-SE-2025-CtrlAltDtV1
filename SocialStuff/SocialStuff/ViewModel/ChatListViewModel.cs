// <copyright file="ChatListViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.View;

    public class ChatListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string searchQuery = string.Empty;

        public ObservableCollection<Chat> ChatList { get; set; }

        public List<Chat> currentUserChats;
        public IChatService chatService;
        public IUserService userService;

        public CountToVisibilityConverter CountToVisibilityConverter { get; set; }

        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.FilterChats();
                }
            }
        }

        public ChatListViewModel(IChatService chatS, IUserService userS)
        {
            this.ChatList = new ObservableCollection<Chat>();
            this.chatService = chatS;
            this.userService = userS;
            this.currentUserChats = this.userService.GetCurrentUserChats();
            this.CountToVisibilityConverter = new CountToVisibilityConverter();

            this.LoadChats();
        }

        public void LoadChats()
        {
            this.FilterChats();
        }

        public void FilterChats()
        {
            this.ChatList.Clear();
            this.currentUserChats = this.userService.GetCurrentUserChats();
            foreach (var chat in this.currentUserChats)
            {
                if (string.IsNullOrEmpty(this.SearchQuery) ||
                    chat.getChatName().IndexOf(this.SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.ChatList.Add(chat);
                }
            }

            // sort chats by last message time
            this.ChatList = new ObservableCollection<Chat>(this.ChatList.OrderByDescending(chat => this.chatService.GetLastMessageTimeStamp(chat.getChatID())));
        }
    }
}
