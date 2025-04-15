// <copyright file="IMessageService.cs" company="PlaceholderCompany">
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
    using SocialStuff.Model.MessageClasses;

    /// <summary>
    /// Defines the contract for message-related operations.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Gets the repository instance.
        /// </summary>
        /// <returns>The repository instance.</returns>
        IRepository GetRepo();

        /// <summary>
        /// Sends a text message.
        /// </summary>
        /// <param name="senderID">The ID of the sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        void SendMessage(int senderID, int chatID, string content);

        /// <summary>
        /// Sends an image message.
        /// </summary>
        /// <param name="senderID">The ID of the sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="imageURL">The URL of the image.</param>
        void SendImage(int senderID, int chatID, string imageURL);

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        void DeleteMessage(Message message);

        /// <summary>
        /// Sends a transfer message.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the transfer.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <param name="currency">The currency of the transfer.</param>
        void SendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency);

        /// <summary>
        /// Sends a request message.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="amount">The requested amount.</param>
        /// <param name="currency">The currency of the request.</param>
        void SendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency);
    }
}
