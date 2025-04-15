// <copyright file="ChatService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Identity.Client;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Model.MessageClasses;
    using SocialStuff.Services.Interfaces;

    /// <summary>
    /// Represents the ChatService class that provides various chat-related functionalities.
    /// </summary>
    public class ChatService : IChatService
    {
        private const int ChatID = 1;
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatService"/> class.
        /// </summary>
        /// <param name="repo">The repository instance to be used by the service.</param>
        public ChatService(IRepository repo)
        {
            this.repository = repo;
        }

        /// <summary>
        /// Gets the ID of the currently logged-in user.
        /// </summary>
        /// <returns>The ID of the currently logged-in user.</returns>
        public int GetCurrentUserID()
        {
            return this.repository.GetLoggedInUserID();
        }

        /// <summary>
        /// Returns the number of participants in a chat excluding the current user.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>The number of participants in the chat excluding the current user.</returns>
        public int GetNumberOfParticipants(int chatID)
        {
            return this.repository.GetChatParticipantsIDs(chatID).Count;
        }

        /// <summary>
        /// Gets the repository instance used by the service.
        /// </summary>
        /// <returns>The repository instance.</returns>
        public IRepository GetRepo()
        {
            return this.repository;
        }

        // MODIFY ERROR REQUEST HAS 1 INSTANCE WITH REQUESTER ID AND CHAT 1 -: DONE (depends on implementation)

        /// <summary>
        /// Requests money from participants in a chat.
        /// </summary>
        /// <param name="amount">The amount of money to request.</param>
        /// <param name="currency">The currency of the request.</param>
        /// <param name="chatID">The ID of the chat where the request is initiated.</param>
        /// <param name="description">A description of the request.</param>
        /// <exception cref="ArgumentException">Thrown when the amount is less than or equal to zero, or the currency is null or empty.</exception>
        public void RequestMoneyViaChat(float amount, string currency, int chatID, string description)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("Currency cannot be null or empty.");
            }

            try
            {
                this.repository.AddRequestMessage(this.GetCurrentUserID(), chatID, description, "Pending", amount, currency);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error requesting money from participant {chatID}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new transfer message and sends money to participants in a chat.
        /// </summary>
        /// <param name="amount">The amount of money to send.</param>
        /// <param name="currency">The currency of the transfer.</param>
        /// <param name="description">A description of the transfer.</param>
        /// <param name="chatID">The ID of the chat where the transfer is initiated.</param>
        /// <exception cref="ArgumentException">Thrown when the amount is less than or equal to zero, or the currency is null or empty.</exception>
        public void SendMoneyViaChat(float amount, string currency, string description, int chatID)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("Currency cannot be null or empty.");
            }

            List<int> participantIDs = this.repository.GetChatParticipantsIDs(chatID);

            try
            {
                if (this.EnoughFunds(amount * (participantIDs.Count - 1), currency, this.GetCurrentUserID()))
                {
                    int currentUserId = this.GetCurrentUserID();
                    foreach (int reciverid in participantIDs)
                    {
                        if (currentUserId != reciverid)
                        {
                            this.InitiateTransfer(currentUserId, reciverid, amount, currency);
                        }
                    }

                    this.repository.AddTransferMessage(this.GetCurrentUserID(), chatID, description, "Accepted", amount * (participantIDs.Count - 1), currency);
                }
                else
                {
                    this.repository.AddTransferMessage(this.GetCurrentUserID(), chatID, description, "Rejected", amount * (participantIDs.Count - 1), currency);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending money to participant {chatID}: {ex.Message}");
            }
        }

        // when a reqest is initiated in chat if you press accept a new transfer message is initiated with sender id the one who pressed and
        // chat id the chat

        /// <summary>
        /// Accepts a money request in a chat by initiating a transfer from the accepter to the requester.
        /// </summary>
        /// <param name="amount">The amount of money to transfer.</param>
        /// <param name="currency">The currency of the transfer.</param>
        /// <param name="accepterID">The ID of the user accepting the request.</param>
        /// <param name="requesterID">The ID of the user who made the request.</param>
        /// <param name="chatID">The ID of the chat where the request was made.</param>
        public void AcceptRequestViaChat(float amount, string currency, int accepterID, int requesterID, int chatID)
        {
            // from Adrada
            if (this.EnoughFunds(amount, currency, accepterID))
            {
                this.InitiateTransfer(accepterID, requesterID, amount, currency);
                this.repository.AddTransferMessage(accepterID, chatID, "YOU JUST SENT " + amount.ToString() + currency + "TO here function that retrives username by ID", "Accepted", amount, currency);
            }
            else
            {
                this.repository.AddTransferMessage(accepterID, chatID, "YOU FAILED TO SEND " + amount.ToString() + currency + "TO here function that retrives username by ID", "Rejected", amount, currency);
            }
        }

        // mock function for enough funds

        /// <summary>
        /// Checks if the sender has enough funds to perform a transaction.
        /// </summary>
        /// <param name="amount">The amount of funds to check.</param>
        /// <param name="currency">The currency of the funds.</param>
        /// <param name="senderID">The ID of the sender.</param>
        /// <returns>True if the sender has enough funds; otherwise, false.</returns>
        public bool EnoughFunds(float amount, string currency, int senderID)
        {
            Random random = new Random();
            bool randomBool = random.Next(2) == 0;
            return randomBool;
        }

        // mock transfer function --- THAT'S WHY IT DOESN'T WORK

        /// <summary>
        /// Initiates a transfer of funds from one user to another.
        /// </summary>
        /// <param name="senderID">The ID of the user sending the funds.</param>
        /// <param name="reciverID">The ID of the user receiving the funds.</param>
        /// <param name="amount">The amount of funds to transfer.</param>
        /// <param name="currency">The currency in which the transfer is made.</param>
        public void InitiateTransfer(int senderID, int reciverID, float amount, string currency)
        {
            return;
        }

        /// <summary>
        /// Creates a new chat with the specified participants and name.
        /// </summary>
        /// <param name="participantsID">A list of user IDs representing the participants of the chat.</param>
        /// <param name="chatName">The name of the chat to be created.</param>
        public void CreateChat(List<int> participantsID, string chatName)
        {
            int chatID = this.repository.AddChat(chatName);

            foreach (var userID in participantsID)
            {
                this.repository.AddUserToChat(userID, chatID);
            }

            var addedUsers = this.repository.GetChatParticipants(chatID);
        }

        /// <summary>
        /// Deletes a chat by its ID.
        /// </summary>
        /// <param name="chatID">The ID of the chat to delete.</param>
        public void DeleteChat(int chatID)
        {
            this.repository.DeleteChat(chatID);
        }

        /// <summary>
        /// Retrieves the timestamp of the last message in a specific chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat whose last message timestamp is to be retrieved.</param>
        /// <returns>The timestamp of the last message in the chat, or <see cref="DateTime.MinValue"/> if no messages are found.</returns>
        public DateTime GetLastMessageTimeStamp(int chatID)
        {
            List<Message> allMessages = this.repository.GetMessagesList();

            var chatMessages = allMessages.Where(m => m.getChatID() == chatID).ToList();

            var lastMessage = chatMessages.OrderByDescending(m => m.getTimestamp()).FirstOrDefault();

            if (lastMessage == null)
            {
                // return a lower datetime
                return DateTime.MinValue;
            }

            return lastMessage.getTimestamp();
        }

        /// <summary>
        /// Retrieves the chat history for a specific chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat whose history is to be retrieved.</param>
        /// <returns>A list of messages representing the chat history.</returns>
        public List<Message> GetChatHistory(int chatID)
        {
            List<Message> allMessages = this.repository.GetMessagesList();

            List<Message> chatHistory = allMessages.Where(m => m.getChatID() == chatID).ToList();

            return chatHistory;
        }

        /// <summary>
        /// Adds a user to a chat.
        /// </summary>
        /// <param name="userID">The ID of the user to add.</param>
        /// <param name="chatID">The ID of the chat to which the user will be added.</param>
        public void AddUserToChat(int userID, int chatID)
        {
            this.repository.AddUserToChat(userID, chatID);
        }

        /// <summary>
        /// Removes a user from a chat.
        /// </summary>
        /// <param name="userID">The ID of the user to remove.</param>
        /// <param name="chatID">The ID of the chat from which the user will be removed.</param>
        public void RemoveUserFromChat(int userID, int chatID)
        {
            this.repository.RemoveUserFromChat(userID, chatID);
        }

        /// <summary>
        /// Retrieves the name of a chat by its ID.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>The name of the chat.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the chat with the specified ID is not found.</exception>
        public string GetChatNameByID(int chatID)
        {
            List<Chat> chatList = this.repository.GetChatsList();
            Chat? chat = chatList.FirstOrDefault(c => c.getChatID() == chatID);
            if (chat == null)
            {
                throw new InvalidOperationException($"Chat with ID {chatID} not found.");
            }

            string chatName = chat.getChatName();
            return chatName;
        }

        /// <summary>
        /// Retrieves the list of participants in a chat as strings (usernames).
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>A list of usernames representing the participants of the chat.</returns>
        public List<string> GetChatParticipantsStringList(int chatID)
        {
            List<User> participants = this.repository.GetChatParticipants(chatID);
            List<string> participantsList = participants.Select(p => p.GetUsername()).ToList();
            return participantsList;
        }

        /// <summary>
        /// Retrieves the list of participants in a chat as User objects.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        /// <returns>A list of User objects representing the participants of the chat.</returns>
        public List<User> GetChatParticipantsList(int chatID)
        {
            List<User> participants = this.repository.GetChatParticipants(chatID);
            return participants;
        }
    }
}
