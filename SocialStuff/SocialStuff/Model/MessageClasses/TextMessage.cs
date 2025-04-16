// <copyright file="TextMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model.MessageClasses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.System;

    /// <summary>
    /// Represents a text message in a chat.
    /// </summary>
    public class TextMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextMessage"/> class with the specified parameters.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="content">The content of the message.</param>
        public TextMessage(int messageID, int senderID, int chatID, string content)
            : base(messageID, senderID, chatID)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = DateTime.Now;
            this.Content = content;
            this.UsersReport = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextMessage"/> class with the specified parameters.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="timestamp">The timestamp of the message.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="usersReport">The list of user IDs who reported the message.</param>
        public TextMessage(int messageID, int senderID, int chatID, DateTime timestamp, string content, List<int> usersReport)
            : base(messageID, senderID, chatID, timestamp)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = timestamp;
            this.Content = content;
            this.UsersReport = new List<int>();
        }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Content { get; set; }

        private int MessageID { get; set; }

        private int SenderID { get; set; }

        private int ChatID { get; set; }

        private DateTime Timestamp { get; set; }

        private List<int> UsersReport { get; set; }

        /// <summary>
        /// Gets the unique identifier of the message.
        /// </summary>
        /// <returns>The message ID.</returns>
        public new int GetMessageID() => this.MessageID;

        /// <summary>
        /// Gets the unique identifier of the sender.
        /// </summary>
        /// <returns>The sender ID.</returns>
        public new int GetSenderID() => this.SenderID;

        /// <summary>
        /// Gets the unique identifier of the chat.
        /// </summary>
        /// <returns>The chat ID.</returns>
        public new int GetChatID() => this.ChatID;

        /// <summary>
        /// Gets the timestamp of the message.
        /// </summary>
        /// <returns>The timestamp.</returns>
        public new DateTime GetTimestamp() => this.Timestamp;

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        /// <returns>The message content.</returns>
        public string GetContent() => this.Content;

        /// <summary>
        /// Gets the list of user IDs who reported the message.
        /// </summary>
        /// <returns>The list of user IDs.</returns>
        public List<int> GetUsersReport() => this.UsersReport;

        /// <summary>
        /// Returns a string representation of the message.
        /// </summary>
        /// <returns>The content of the message.</returns>
        public override string ToString()
        {
            return this.Content;
        }
    }
}