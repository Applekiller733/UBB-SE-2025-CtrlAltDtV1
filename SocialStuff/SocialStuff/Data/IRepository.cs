// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SocialStuff.Model;
    using SocialStuff.Model.MessageClasses;
    using SocialStuff.Services;
    using NotificationModel = SocialStuff.Model.Notification;

    /// <summary>
    /// Represents a repository interface for managing users, chats, messages, notifications, reports, and feed posts.
    /// </summary>
    public interface IRepository
    {
        // user part

        /// <summary>
        /// Gets the ID of the currently logged-in user.
        /// </summary>
        /// <returns>The ID of the logged-in user.</returns>
        int GetLoggedInUserID();

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
        User GetUserById(int userId);

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        List<User> GetUsersList();

        /// <summary>
        /// Retrieves a list of friends for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose friends are to be retrieved.</param>
        /// <returns>A list of the user's friends.</returns>
        List<User> GetUserFriendsList(int userId);

        /// <summary>
        /// Retrieves a list of friend IDs for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose friend IDs are to be retrieved.</param>
        /// <returns>A list of friend IDs.</returns>
        List<int> GetFriendsIDs(int userId);

        /// <summary>
        /// Adds a friend to a user's friend list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="friendId">The ID of the friend to add.</param>
        void AddFriend(int userId, int friendId);

        /// <summary>
        /// Removes a friend from a user's friend list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="friendId">The ID of the friend to remove.</param>
        void DeleteFriend(int userId, int friendId);

        // chat part

        /// <summary>
        /// Retrieves a chat by its ID.
        /// </summary>
        /// <param name="chatId">The ID of the chat to retrieve.</param>
        /// <returns>The chat with the specified ID, or null if not found.</returns>
        Chat? GetChatById(int chatId);

        /// <summary>
        /// Retrieves a list of all chats.
        /// </summary>
        /// <returns>A list of all chats.</returns>
        List<Chat> GetChatsList();

        /// <summary>
        /// Retrieves the participants of a specific chat.
        /// </summary>
        /// <param name="chatId">The ID of the chat.</param>
        /// <returns>A list of users participating in the chat.</returns>
        List<User> GetChatParticipants(int chatId);

        /// <summary>
        /// Retrieves the IDs of participants in a specific chat.
        /// </summary>
        /// <param name="chatId">The ID of the chat.</param>
        /// <returns>A list of participant IDs.</returns>
        List<int> GetChatParticipantsIDs(int chatId);

        /// <summary>
        /// Retrieves the IDs of chats a user is part of.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of chat IDs.</returns>
        List<int> GetChatsIDs(int userId);

        /// <summary>
        /// Adds a new chat with the specified name.
        /// </summary>
        /// <param name="chatName">The name of the chat.</param>
        /// <returns>The ID of the newly created chat.</returns>
        int AddChat(string chatName);

        /// <summary>
        /// Deletes a chat by its ID.
        /// </summary>
        /// <param name="chatId">The ID of the chat to delete.</param>
        void DeleteChat(int chatId);

        /// <summary>
        /// Adds a user to a specific chat.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <param name="chatId">The ID of the chat.</param>
        void AddUserToChat(int userId, int chatId);

        /// <summary>
        /// Removes a user from a specific chat.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        /// <param name="chatId">The ID of the chat.</param>
        void RemoveUserFromChat(int userId, int chatId);

        // message part

        /// <summary>
        /// Retrieves a list of all messages.
        /// </summary>
        /// <returns>A list of all messages.</returns>
        List<Message> GetMessagesList();

        /// <summary>
        /// Adds a text message to a chat.
        /// </summary>
        /// <param name="userId">The ID of the sender.</param>
        /// <param name="chatId">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        void AddTextMessage(int userId, int chatId, string content);

        /// <summary>
        /// Adds an image message to a chat.
        /// </summary>
        /// <param name="userId">The ID of the sender.</param>
        /// <param name="chatId">The ID of the chat.</param>
        /// <param name="imageURL">The URL of the image.</param>
        void AddImageMessage(int userId, int chatId, string imageURL);

        /// <summary>
        /// Adds a request message to a chat.
        /// </summary>
        /// <param name="userId">The ID of the sender.</param>
        /// <param name="chatId">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the request (optional).</param>
        /// <param name="amount">The amount associated with the request (optional).</param>
        /// <param name="currency">The currency of the amount (optional).</param>
        void AddRequestMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null);

        /// <summary>
        /// Adds a transfer message to a chat.
        /// </summary>
        /// <param name="userId">The ID of the sender.</param>
        /// <param name="chatId">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the transfer (optional).</param>
        /// <param name="amount">The amount associated with the transfer (optional).</param>
        /// <param name="currency">The currency of the amount (optional).</param>
        void AddTransferMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null);

        /// <summary>
        /// Deletes a message by its ID.
        /// </summary>
        /// <param name="messageId">The ID of the message to delete.</param>
        void DeleteMessage(int messageId);

        // notification part

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of notifications.</returns>
        List<NotificationModel> GetNotifications(int userId);

        /// <summary>
        /// Adds a notification for a user.
        /// </summary>
        /// <param name="content">The content of the notification.</param>
        /// <param name="userId">The ID of the user to notify.</param>
        void AddNotification(string content, int userId);

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to delete.</param>
        void DeleteNotification(int notificationId);

        /// <summary>
        /// Clears all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        void ClearAllNotifications(int userId);

        // report part

        /// <summary>
        /// Retrieves a list of all reports.
        /// </summary>
        /// <returns>A list of reports.</returns>
        List<Report> GetReportsList();

        /// <summary>
        /// Adds a report for a specific message.
        /// </summary>
        /// <param name="messageId">The ID of the message being reported.</param>
        /// <param name="reason">The reason for the report.</param>
        /// <param name="description">A description of the report.</param>
        /// <param name="status">The status of the report.</param>
        void AddReport(int messageId, string reason, string description, string status);

        // feed part

        /// <summary>
        /// Retrieves a list of feed posts.
        /// </summary>
        /// <returns>A list of feed posts.</returns>
        List<Post> GetFeedPostsList();
    }
}
