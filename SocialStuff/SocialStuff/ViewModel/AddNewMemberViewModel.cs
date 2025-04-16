// <copyright file="AddNewMemberViewModel.cs" company="PlaceholderCompany">
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
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class AddNewMemberViewModel : INotifyPropertyChanged
    {
        private List<User> allUnaddedFriends;
        private IUserService userService;
        private IChatService chatService;
        private Page lastChat;
        private string searchQuery;
        private int ChatID;
        private ChatMessagesViewModel chatMessagesViewModel;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> UnaddedFriends { get; set; }

        public ObservableCollection<User> CurrentChatMembers { get; set; }

        public ObservableCollection<User> NewlyAddedFriends { get; set; }

        public string ChatName { get; set; }

        public ICommand AddToSelectedCommand { get; set; }

        public ICommand RemoveFromSelectedCommand { get; set; }

        public ICommand AddUsersToChatCommand { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddNewMemberViewModel(ChatMessagesViewModel chatMessagesViewModel, Page lastChat, int ChatID, IChatService chat, IUserService user)
        {
            this.chatMessagesViewModel = chatMessagesViewModel;
            this.ChatID = ChatID;
            this.lastChat = lastChat;
            this.userService = user;
            this.chatService = chat;
            this.ChatName = this.chatService.GetChatNameByID(ChatID);

            this.UnaddedFriends = new ObservableCollection<User>();
            this.CurrentChatMembers = new ObservableCollection<User>();
            this.NewlyAddedFriends = new ObservableCollection<User>();

            this.AddToSelectedCommand = new RelayCommand<User>(this.AddToSelected);
            this.RemoveFromSelectedCommand = new RelayCommand<User>(this.RemoveFromSelected);
            this.AddUsersToChatCommand = new RelayCommand(this.AddUsersToChat);

            this.UpdateObservableLists();
        }

        public void AddUsersToChat()
        {
            foreach (User user in this.NewlyAddedFriends)
            {
                this.chatService.AddUserToChat(user.GetUserId(), this.ChatID);
            }

            this.NewlyAddedFriends.Clear();
            this.UpdateObservableLists();

            this.chatMessagesViewModel.CurrentChatParticipants = this.chatService.GetChatParticipantsStringList(ChatID);
        }

        public void AddToSelected(User user)
        {
            this.NewlyAddedFriends.Add(user);
            this.UnaddedFriends.Remove(user);
        }

        public void RemoveFromSelected(User user)
        {
            this.NewlyAddedFriends.Remove(user);
            this.UnaddedFriends.Add(user);
        }

        public string SearchQuery
        {
            get
            {
                return this.searchQuery;
            }

            set
            {
                if (this.searchQuery != value)
                {
                    this.searchQuery = value;
                    this.OnPropertyChanged(nameof(this.SearchQuery));
                    this.UpdateFilteredFriends();
                }
            }
        }

        public void LoadAllUnaddedFriendsList()
        {
            var allFriends = this.userService.GetFriendsByUser(this.userService.GetCurrentUser());
            var currentChatParticipants = this.chatService.GetChatParticipantsList(this.ChatID);
            this.allUnaddedFriends = allFriends.Where(friend => !currentChatParticipants.Any(participant => participant.GetUserId() == friend.GetUserId())).ToList();
        }

        public void UpdateObservableLists()
        {
            this.LoadAllUnaddedFriendsList();

            this.CurrentChatMembers.Clear();
            foreach (var participant in this.chatService.GetChatParticipantsList(this.ChatID))
            {
                this.CurrentChatMembers.Add(participant);
            }

            this.UpdateFilteredFriends();
        }

        public void UpdateFilteredFriends()
        {
            this.UnaddedFriends.Clear();
            foreach (var friend in this.allUnaddedFriends.Where(f =>
                 string.IsNullOrEmpty(this.SearchQuery) ||
                 f.GetUsername()?.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) == true ||
                 f.GetPhoneNumber()?.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase) == true))
            {
                this.UnaddedFriends.Add(friend);
            }
        }
    }
}