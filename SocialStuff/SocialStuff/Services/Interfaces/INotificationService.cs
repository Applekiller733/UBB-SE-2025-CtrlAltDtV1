// <copyright file="INotificationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NotificationModel = SocialStuff.Model.Notification;

    /// <summary>
    /// Defines methods for managing notifications in the application.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves a list of notifications for a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of notifications.</returns>
        List<NotificationModel> GetNotifications(int userID);

        /// <summary>
        /// Sends a notification when a new friend is added.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="newFriendID">The ID of the new friend.</param>
        void SendFriendNotification(int userID, int newFriendID);

        /// <summary>
        /// Sends a notification when a friend is removed.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="oldFriendID">The ID of the removed friend.</param>
        void SendRemoveFriendNotification(int userID, int oldFriendID);

        /// <summary>
        /// Sends a notification for a new message in a chat.
        /// </summary>
        /// <param name="messageSenderID">The ID of the message sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void SendMessageNotification(int messageSenderID, int chatID);

        /// <summary>
        /// Sends a transaction notification.
        /// </summary>
        /// <param name="receiverID">The ID of the receiver.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="type">The type of transaction.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="currency">The currency of the transaction.</param>
        void SendTransactionNotification(int receiverID, int chatID, string type, float amount, string currency);

        /// <summary>
        /// Sends a notification for a new chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        void SendNewChatNotification(int chatID);

        /// <summary>
        /// Clears a specific notification.
        /// </summary>
        /// <param name="notificationID">The ID of the notification to clear.</param>
        void ClearNotification(int notificationID);

        /// <summary>
        /// Clears all notifications for a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        void ClearAllNotifications(int userID);
    }
}
