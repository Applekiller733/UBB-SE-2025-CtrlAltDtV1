// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Data.SqlClient;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    /// <summary>
    /// Provides user-related services such as managing friends, chats, and filtering users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly INotificationService notificationService;
        private IRepository repo;
        private int userID;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repo">The repository for data access.</param>
        /// <param name="notificationService">The notification service for sending notifications.</param>
        public UserService(IRepository repo, INotificationService notificationService)
        {
            this.repo = repo;
            this.notificationService = notificationService;
            this.userID = this.GetCurrentUser();
        }

        /// <summary>
        /// Gets the repository instance.
        /// </summary>
        /// <returns>The repository instance.</returns>
        public IRepository GetRepo()
        {
            return this.repo;
        }

        /// <summary>
        /// Adds a new friend for the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="newFriendID">The ID of the new friend to add.</param>
        public void AddFriend(int userID, int newFriendID)
        {
            var user = this.GetUserById(userID);
            var friend = this.GetUserById(newFriendID);
            var friends = this.repo.GetFriendsIDs(userID);

            if (user != null && friend != null && !friends.Contains(newFriendID))
            {
                this.repo.AddFriend(userID, newFriendID);
                user.AddFriend(newFriendID);
                this.notificationService.SendFriendNotification(userID, newFriendID);
            }
        }

        /// <summary>
        /// Removes a friend for the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="oldFriendID">The ID of the friend to remove.</param>
        public void RemoveFriend(int userID, int oldFriendID)
        {
            var user = this.GetUserById(userID);
            var friend = this.GetUserById(oldFriendID);
            var friends = this.repo.GetFriendsIDs(userID);

            if (user != null && friend != null && friends.Contains(oldFriendID))
            {
                this.repo.DeleteFriend(userID, oldFriendID);
                user.RemoveFriend(oldFriendID);
                this.notificationService.SendRemoveFriendNotification(userID, oldFriendID);
            }
        }

        /// <summary>
        /// Allows a user to join a chat if they are not already a participant.
        /// </summary>
        /// <param name="userID">The ID of the user attempting to join the chat.</param>
        /// <param name="chatID">The ID of the chat the user wants to join.</param>
        public void JoinChat(int userID, int chatID)
        {
            var user = this.GetUserById(userID);
            var chats = this.repo.GetChatsIDs(userID);

            if (user != null && !chats.Contains(chatID))
            {
                this.repo.AddUserToChat(chatID, userID);
                user.JoinChat(chatID);
            }
        }

        /// <summary>
        /// Allows a user to leave a chat if they are a participant.
        /// </summary>
        /// <param name="userID">The ID of the user attempting to leave the chat.</param>
        /// <param name="chatID">The ID of the chat the user wants to leave.</param>
        public void LeaveChat(int userID, int chatID)
        {
            var user = this.GetUserById(userID);
            var chats = this.repo.GetChatsIDs(userID);

            if (user != null && chats.Contains(chatID))
            {
                this.repo.RemoveUserFromChat(userID, chatID);
                user.LeaveChat(chatID);
            }
        }

        /// <summary>
        /// Filters users based on a keyword and excludes the specified user.
        /// </summary>
        /// <param name="keyword">The keyword to filter users by.</param>
        /// <param name="userID">The ID of the user to exclude from the results.</param>
        /// <returns>A list of user IDs that match the keyword and are not the specified user.</returns>
        public List<int> FilterUsers(string keyword, int userID)
        {
            var users = this.repo.GetUsersList();
            return users.Where(u => (u.GetUsername().Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || u.GetPhoneNumber().Contains(keyword)) && u.GetUserId() != userID)
                        .Select(u => u.GetUserId())
                        .ToList();
        }

        /// <summary>
        /// Filters the friends of a user based on a keyword.
        /// </summary>
        /// <param name="keyword">The keyword to filter friends by.</param>
        /// <param name="userID">The ID of the user whose friends are being filtered.</param>
        /// <returns>A list of friend IDs that match the keyword.</returns>
        public List<int> FilterFriends(string keyword, int userID)
        {
            var user = this.GetUserById(userID);
            if (user == null)
            {
                return new List<int>();
            }

            var friends = this.repo.GetUserFriendsList(userID);

            return friends
                       .Select(friendID => friendID)
                       .Where(friend => friend != null &&
                                        (friend.GetUsername().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         friend.GetPhoneNumber().Contains(keyword)))
                       .Select(friend => friend.GetUserId())
                       .ToList();
        }

        /// <summary>
        /// Retrieves the IDs of all friends for the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user whose friends' IDs are being retrieved.</param>
        /// <returns>A list of friend IDs for the specified user.</returns>
        public List<int> GetFriendsIDsByUser(int userID)
        {
            var friends = this.repo.GetFriendsIDs(userID);

            return friends;
        }

        /// <summary>
        /// Retrieves the list of friends for the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user whose friends are being retrieved.</param>
        /// <returns>A list of <see cref="User"/> objects representing the user's friends.</returns>
        public List<User> GetFriendsByUser(int userID)
        {
            return this.repo.GetUserFriendsList(userID);
        }

        /// <summary>
        /// Retrieves the IDs of all chats for the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user whose chat IDs are being retrieved.</param>
        /// <returns>A list of chat IDs for the specified user.</returns>
        public List<int> GetChatsByUser(int userID)
        {
            var chats = this.repo.GetChatsIDs(userID);
            return chats;
        }

        /// <summary>
        /// Retrieves the list of chats for the currently logged-in user.
        /// </summary>
        /// <returns>A list of <see cref="Chat"/> objects representing the chats the current user is part of.</returns>
        public List<Chat> GetCurrentUserChats()
        {
            List<Chat> chats = new List<Chat>();
            List<Chat> currentUserChats = new List<Chat>();
            chats = this.repo.GetChatsList();

            foreach (Chat chat in chats)
            {
                if (chat.getUserIDsList().Contains(this.userID))
                {
                    currentUserChats.Add(chat);
                }
            }

            return currentUserChats;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userID">The ID of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> object if found; otherwise, a new <see cref="User"/> object.</returns>
        public User GetUserById(int userID)
        {
            List<User> users = this.repo.GetUsersList();
            foreach (User user in users)
            {
                if (user.GetUserId() == userID)
                {
                    return user;
                }
            }

            return new User();
        }

        /// <summary>
        /// Retrieves a list of users who are not friends with the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user whose non-friends are being retrieved.</param>
        /// <returns>A list of <see cref="User"/> objects representing users who are not friends with the specified user.</returns>
        public List<User> GetNonFriendsUsers(int userID)
        {
            List<User> users = new List<User>(this.repo.GetUsersList().Where(user => user.GetUserId() != userID));
            List<int> friends = this.repo.GetFriendsIDs(userID);
            List<User> nonFriends = new List<User>();
            foreach (User user in users)
            {
                if (!friends.Contains(user.GetUserId()))
                {
                    nonFriends.Add(user);
                }
            }

            return nonFriends;
        }

        /// <summary>
        /// Retrieves the ID of the currently logged-in user.
        /// </summary>
        /// <returns>The ID of the currently logged-in user.</returns>
        public int GetCurrentUser()
        {
            return this.repo.GetLoggedInUserID(); // This should be replaced with actual logic to get the logged-in user.
        }

        /// <summary>
        /// Marks a user as dangerous and assigns a timeout period.
        /// </summary>
        /// <param name="user">The user to be marked as dangerous.</param>
        public void MarkUserAsDangerousAndGiveTimeout(User user)
        {
            if (user.GetReportedCount() >= 1)
            {
                user.SetTimeoutEnd(DateTime.Now.AddMinutes(3));
            }
        }

        /// <summary>
        /// Checks if the specified user is currently in a timeout period.
        /// </summary>
        /// <param name="user">The user to check for timeout status.</param>
        /// <returns>True if the user is in timeout; otherwise, false.</returns>
        public bool IsUserInTimeout(User user)
        {
            return user.GetTimeoutEnd() != null && user.GetTimeoutEnd() > DateTime.Now;
        }

        /*public void setUserTimeout(bool value)
        {
            this.isUserInTimeout = value;
        }
        */
    }
}