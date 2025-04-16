// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model.MessageClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract base class for messages. Provides common properties and methods for all message types.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class with the specified message ID, sender ID, and chat ID.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        public Message(int messageID, int senderID, int chatID)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = DateTime.Now;
            this.SenderUsername = string.Empty; // Initialize with a default value
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class with the specified message ID, sender ID, chat ID, and timestamp.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="timestamp">The timestamp of when the message was created.</param>
        public Message(int messageID, int senderID, int chatID, DateTime timestamp)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = timestamp;
            this.SenderUsername = string.Empty; // Initialize with a default value
        }

        /// <summary>
        /// Gets or sets the username of the sender.
        /// </summary>
        public string SenderUsername { get; set; }

        private int MessageID { get; set; }

        private int SenderID { get; set; }

        private int ChatID { get; set; }

        private DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets the unique identifier of the message.
        /// </summary>
        /// <returns>The message ID.</returns>
        public int GetMessageID() => this.MessageID;

        /// <summary>
        /// Gets the unique identifier of the sender.
        /// </summary>
        /// <returns>The sender ID.</returns>
        public int GetSenderID() => this.SenderID;

        /// <summary>
        /// Gets the unique identifier of the chat.
        /// </summary>
        /// <returns>The chat ID.</returns>
        public int GetChatID() => this.ChatID;

        /// <summary>
        /// Gets the timestamp of when the message was created.
        /// </summary>
        /// <returns>The timestamp.</returns>
        public DateTime GetTimestamp() => this.Timestamp;

        /// <summary>
        /// Returns a string representation of the message.
        /// </summary>
        /// <returns>A string describing the message.</returns>
        public override string ToString()
        {
            return $"{this.GetType().Name} from {this.SenderID} in chat {this.ChatID} at {this.Timestamp}";
        }
    }
}