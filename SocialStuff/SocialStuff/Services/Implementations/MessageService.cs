// <copyright file="MessageService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Model.MessageClasses;
    using SocialStuff.Services.Interfaces;

    /// <summary>
    /// Provides services for managing messages, including sending, deleting, and reporting messages.
    /// </summary>
    public class MessageService : IMessageService
    {
        private IRepository repository;
        private IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="repo">The repository instance for data operations.</param>
        /// <param name="userService">The user service instance for user-related operations.</param>
        public MessageService(IRepository repo, IUserService userService)
        {
            this.repository = repo;
            this.userService = userService;
        }

        /// <summary>
        /// Gets the repository instance.
        /// </summary>
        /// <returns>The repository instance.</returns>
        public IRepository getRepo()
        {
            return this.repository;
        }

        /// <summary>
        /// Sends a text message to a chat.
        /// </summary>
        /// <param name="SenderID">The ID of the sender.</param>
        /// <param name="ChatID">The ID of the chat.</param>
        /// <param name="Content">The content of the message.</param>
        public void sendMessage(int SenderID, int ChatID, string Content)
        {
            if (this.IsUserInTimeout(SenderID))
            {
                return;
            }

            this.repository.AddTextMessage(SenderID, ChatID, Content);
        }

        /// <summary>
        /// Sends an image message to a chat.
        /// </summary>
        /// <param name="SenderID">The ID of the sender.</param>
        /// <param name="ChatID">The ID of the chat.</param>
        /// <param name="ImageURL">The URL of the image.</param>
        public void sendImage(int SenderID, int ChatID, string ImageURL)
        {
            if (this.IsUserInTimeout(SenderID))
            {
                return;
            }

            this.repository.AddImageMessage(SenderID, ChatID, ImageURL);
        }

        /// <summary>
        /// Deletes a message from the repository.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        public void deleteMessage(Message message)
        {
            this.repository.DeleteMessage(message.getMessageID());
        }

        /// <summary>
        /// Sends a transfer message to a chat.
        /// </summary>
        /// <param name="userID">The ID of the user sending the message.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the transfer.</param>
        /// <param name="amount">The amount being transferred.</param>
        /// <param name="currency">The currency of the transfer.</param>
        public void sendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            this.repository.AddTransferMessage(userID, chatID, content, status, amount, currency);
        }

        /// <summary>
        /// Sends a request message to a chat.
        /// </summary>
        /// <param name="userID">The ID of the user sending the message.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="amount">The amount being requested.</param>
        /// <param name="currency">The currency of the request.</param>
        public void sendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            this.repository.AddRequestMessage(userID, chatID, content, status, amount, currency);
        }

        /// <summary>
        /// Reports a message.
        /// </summary>
        /// <param name="message">The message to report.</param>
        internal void ReportMessage(Message message)
        {
            // this.repository.(message.getMessageID());
        }

        /// <summary>
        /// Checks if a user is in timeout.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <returns>True if the user is in timeout; otherwise, false.</returns>
        private bool IsUserInTimeout(int userID)
        {
            User user = this.userService.GetUserById(userID);
            bool isInTimeout = user != null && this.userService.IsUserInTimeout(user);
            System.Diagnostics.Debug.WriteLine($"MessageService checking if user {user?.GetUsername()} is in timeout: {isInTimeout}");
            return isInTimeout;
        }
    }
}