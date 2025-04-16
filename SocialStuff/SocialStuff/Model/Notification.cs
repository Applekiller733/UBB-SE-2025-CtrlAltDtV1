// <copyright file="Notification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a notification sent to a user.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="notificationID">The unique identifier for the notification.</param>
        /// <param name="timestamp">The timestamp when the notification was created.</param>
        /// <param name="content">The content of the notification.</param>
        /// <param name="userReceiverID">The ID of the user receiving the notification.</param>
        public Notification(int notificationID, DateTime timestamp, string content, int userReceiverID)
        {
            this.NotificationID = notificationID;
            this.Timestamp = timestamp;
            this.Content = content;
            this.UserReceiverID = userReceiverID;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the notification.
        /// </summary>
        public int NotificationID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the notification was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user receiving the notification.
        /// </summary>
        public int UserReceiverID { get; set; }

        /// <summary>
        /// Gets the content of the notification.
        /// </summary>
        /// <returns>The content of the notification.</returns>
        public string GetContent()
        {
            return this.Content;
        }

        /// <summary>
        /// Gets the ID of the user receiving the notification.
        /// </summary>
        /// <returns>The ID of the user receiving the notification.</returns>
        public int GetReceiverID()
        {
            return this.UserReceiverID;
        }

        /// <summary>
        /// Gets the timestamp when the notification was created.
        /// </summary>
        /// <returns>The timestamp of the notification.</returns>
        public DateTime GetTimestamp()
        {
            return this.Timestamp;
        }
    }
}