// <copyright file="IChatService.cs" company="PlaceholderCompany">
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
    using SocialStuff.Model.MessageClasses;

    /// <summary>
    /// Defines the contract for chat-related operations.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Gets the ID of the current user.
        /// </summary>
        /// <returns>The ID of the current user.</returns>
        int GetCurrentUserID();

        /// <summary>
        /// Gets the number of participants in a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>The number of participants in the chat.</returns>
        int GetNumberOfParticipants(int chatID);

        /// <summary>
        /// Gets the repository instance.
        /// </summary>
        /// <returns>The repository instance.</returns>
        IRepository GetRepo();

        /// <summary>
        /// Requests money via chat.
        /// </summary>
        /// <param name="amount">The amount to request.</param>
        /// <param name="currency">The currency of the amount.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="description">The description of the request.</param>
        void RequestMoneyViaChat(float amount, string currency, int chatID, string description);

        /// <summary>
        /// Sends money via chat.
        /// </summary>
        /// <param name="amount">The amount to send.</param>
        /// <param name="currency">The currency of the amount.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void SendMoneyViaChat(float amount, string currency, string description, int chatID);

        /// <summary>
        /// Accepts a money request via chat.
        /// </summary>
        /// <param name="amount">The amount to accept.</param>
        /// <param name="currency">The currency of the amount.</param>
        /// <param name="accepterID">The ID of the accepter.</param>
        /// <param name="requesterID">The ID of the requester.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void AcceptRequestViaChat(float amount, string currency, int accepterID, int requesterID, int chatID);

        /// <summary>
        /// Checks if the sender has enough funds.
        /// </summary>
        /// <param name="amount">The amount to check.</param>
        /// <param name="currency">The currency of the amount.</param>
        /// <param name="senderID">The ID of the sender.</param>
        /// <returns>True if the sender has enough funds; otherwise, false.</returns>
        bool EnoughFunds(float amount, string currency, int senderID);

        /// <summary>
        /// Initiates a transfer between two users.
        /// </summary>
        /// <param name="senderID">The ID of the sender.</param>
        /// <param name="reciverID">The ID of the receiver.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <param name="currency">The currency of the amount.</param>
        void InitiateTransfer(int senderID, int reciverID, float amount, string currency);

        /// <summary>
        /// Creates a new chat.
        /// </summary>
        /// <param name="participantsID">The list of participant IDs.</param>
        /// <param name="chatName">The name of the chat.</param>
        void CreateChat(List<int> participantsID, string chatName);

        /// <summary>
        /// Deletes a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat to delete.</param>
        void DeleteChat(int chatID);

        /// <summary>
        /// Gets the timestamp of the last message in a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>The timestamp of the last message.</returns>
        DateTime GetLastMessageTimeStamp(int chatID);

        /// <summary>
        /// Gets the chat history.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>A list of messages in the chat.</returns>
        List<Message> GetChatHistory(int chatID);

        /// <summary>
        /// Adds a user to a chat.
        /// </summary>
        /// <param name="userID">The ID of the user to add.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void AddUserToChat(int userID, int chatID);

        /// <summary>
        /// Removes a user from a chat.
        /// </summary>
        /// <param name="userID">The ID of the user to remove.</param>
        /// <param name="chatID">The ID of the chat.</param>
        void RemoveUserFromChat(int userID, int chatID);

        /// <summary>
        /// Gets the name of a chat by its ID.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>The name of the chat.</returns>
        string GetChatNameByID(int chatID);

        /// <summary>
        /// Gets a list of participant names in a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>A list of participant names.</returns>
        List<string> GetChatParticipantsStringList(int chatID);

        /// <summary>
        /// Gets a list of participant users in a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>A list of participant users.</returns>
        List<User> GetChatParticipantsList(int chatID);
    }
}
