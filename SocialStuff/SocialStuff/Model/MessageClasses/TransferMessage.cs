// <copyright file="TransferMessage.cs" company="PlaceholderCompany">
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
    /// Represents a transfer message that contains details about a monetary transfer.
    /// </summary>
    public class TransferMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferMessage"/> class with the specified parameters.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="status">The status of the transfer.</param>
        /// <param name="amount">The amount of the transfer.</param>
        /// <param name="desc">The description of the transfer.</param>
        /// <param name="currency">The currency of the transfer.</param>
        public TransferMessage(int messageID, int senderID, int chatID, string status, float amount, string desc, string currency)
            : base(messageID, senderID, chatID)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = DateTime.Now;
            this.Status = status;
            this.Amount = amount;
            this.Description = desc;
            this.Currency = currency;
            this.ListOfReceiversID = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferMessage"/> class with the specified parameters, including a timestamp.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="senderID">The unique identifier of the sender.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="timestamp">The timestamp of the message.</param>
        /// <param name="status">The status of the transfer.</param>
        /// <param name="amount">The amount of the transfer.</param>
        /// <param name="desc">The description of the transfer.</param>
        /// <param name="currency">The currency of the transfer.</param>
        public TransferMessage(int messageID, int senderID, int chatID, DateTime timestamp, string status, float amount, string desc, string currency)
            : base(messageID, senderID, chatID, timestamp)
        {
            this.MessageID = messageID;
            this.SenderID = senderID;
            this.ChatID = chatID;
            this.Timestamp = timestamp;
            this.Status = status;
            this.Amount = amount;
            this.Description = desc;
            this.Currency = currency;
            this.ListOfReceiversID = new List<int>();
        }

        /// <summary>
        /// Gets or sets the amount of the transfer.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Gets or sets the description of the transfer.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the currency of the transfer.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets the formatted amount with the currency.
        /// </summary>
        public string FormattedAmount => $"{this.Amount} {this.Currency}";

        private int MessageID { get; set; }

        private int SenderID { get; set; }

        private int ChatID { get; set; }

        private DateTime Timestamp { get; set; }

        private string Status { get; set; }

        private List<int> ListOfReceiversID { get; set; }

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
        /// Gets the timestamp of the message.
        /// </summary>
        /// <returns>The timestamp.</returns>
        public DateTime GetTimestamp() => this.Timestamp;

        /// <summary>
        /// Gets the status of the transfer.
        /// </summary>
        /// <returns>The status.</returns>
        public string GetStatus() => this.Status;

        /// <summary>
        /// Gets the amount of the transfer.
        /// </summary>
        /// <returns>The amount.</returns>
        public float GetAmount() => this.Amount;

        /// <summary>
        /// Gets the description of the transfer.
        /// </summary>
        /// <returns>The description.</returns>
        public string GetDescription() => this.Description;

        /// <summary>
        /// Gets the currency of the transfer.
        /// </summary>
        /// <returns>The currency.</returns>
        public string GetCurrency() => this.Currency;

        /// <summary>
        /// Gets the list of receiver IDs for the transfer.
        /// </summary>
        /// <returns>The list of receiver IDs.</returns>
        public List<int> GetListOfReceiversID() => this.ListOfReceiversID;

        /// <summary>
        /// Returns a string representation of the transfer message.
        /// </summary>
        /// <returns>A string describing the transfer message.</returns>
        public override string ToString()
        {
            return $"Transfer Message: {this.Amount}{this.Currency} - {this.Description}";
        }
    }
}