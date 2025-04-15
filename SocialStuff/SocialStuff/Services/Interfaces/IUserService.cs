// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SocialStuff.Data;
    using SocialStuff.Model;

    /// <summary>
    /// Provides user-related services such as managing friends, chats, and user information.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the repository instance.
        /// </summary>
        /// <returns>The repository instance.</returns>
        IRepository GetRepo();

        /// <summary>
        /// Adds a friend to the user's friend list.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="newFriendID">The ID of the new friend.</param>
        void AddFriend(int userID, int newFriendID);

        /// <summary>
        /// Removes a friend from the user's friend list.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="oldFriendID">The ID of the friend to remove.</param>
        void RemoveFriend(int userID, int oldFriendID);

        /// <summary>
        /// Adds the user to a chat.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void JoinChat(int userID, int chatID);

        /// <summary>
        /// Removes the user from a chat.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void LeaveChat(int userID, int chatID);

        /// <summary>
        /// Filters users based on a keyword and excludes the specified user.
        /// </summary>
        /// <param name="keyword">The keyword to filter users by.</param>
        /// <param name="userID">The ID of the user to exclude.</param>
        /// <returns>A list of user IDs matching the filter.</returns>
        List<int> FilterUsers(string keyword, int userID);

        /// <summary>
        /// Filters the user's friends based on a keyword.
        /// </summary>
        /// <param name="keyword">The keyword to filter friends by.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of friend IDs matching the filter.</returns>
        List<int> FilterFriends(string keyword, int userID);

        /// <summary>
        /// Gets the IDs of the user's friends.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of friend IDs.</returns>
        List<int> GetFriendsIDsByUser(int userID);

        /// <summary>
        /// Gets the user's friends as a list of User objects.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of User objects representing the user's friends.</returns>
        List<User> GetFriendsByUser(int userID);

        /// <summary>
        /// Gets the IDs of the chats the user is part of.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of chat IDs.</returns>
        List<int> GetChatsByUser(int userID);

        /// <summary>
        /// Gets the chats the current user is part of.
        /// </summary>
        /// <returns>A list of Chat objects.</returns>
        List<Chat> GetCurrentUserChats();

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>The User object.</returns>
        User GetUserById(int userID);

        /// <summary>
        /// Gets a list of users who are not friends with the specified user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of User objects.</returns>
        List<User> GetNonFriendsUsers(int userID);

        /// <summary>
        /// Gets the ID of the current user.
        /// </summary>
        /// <returns>The ID of the current user.</returns>
        int GetCurrentUser();

        /// <summary>
        /// Marks a user as dangerous and applies a timeout.
        /// </summary>
        /// <param name="user">The User object to mark as dangerous.</param>
        void MarkUserAsDangerousAndGiveTimeout(User user);

        /// <summary>
        /// Checks if a user is currently in timeout.
        /// </summary>
        /// <param name="user">The User object to check.</param>
        /// <returns>True if the user is in timeout; otherwise, false.</returns>
        bool IsUserInTimeout(User user);

        // void setUserTimeout(bool value);
    }
}
