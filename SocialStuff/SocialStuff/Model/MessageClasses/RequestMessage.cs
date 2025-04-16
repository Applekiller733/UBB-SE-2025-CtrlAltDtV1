// <copyright file="RequestMessage.cs" company="PlaceholderCompany">
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
    /// Represents a request message that extends the base <see cref="Message"/> class.
    /// </summary>
    public class RequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage"/> class with the specified parameters.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="requesterID">The unique identifier of the requester.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="amount">The amount associated with the request.</param>
        /// <param name="desc">The description of the request.</param>
        /// <param name="currency">The currency of the amount.</param>
        public RequestMessage(int messageID, int requesterID, int chatID, string status, float amount, string desc, string currency)
            : base(messageID, requesterID, chatID)
        {
            this.MessageID = messageID;
            this.SenderID = requesterID;
            this.ChatID = chatID;
            this.Timestamp = DateTime.Now;
            this.Status = status;
            this.Amount = amount;
            this.Description = desc;
            this.Currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage"/> class with the specified parameters, including a timestamp.
        /// </summary>
        /// <param name="messageID">The unique identifier of the message.</param>
        /// <param name="requesterID">The unique identifier of the requester.</param>
        /// <param name="chatID">The unique identifier of the chat.</param>
        /// <param name="timestamp">The timestamp of the message.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="amount">The amount associated with the request.</param>
        /// <param name="desc">The description of the request.</param>
        /// <param name="currency">The currency of the amount.</param>
        public RequestMessage(int messageID, int requesterID, int chatID, DateTime timestamp, string status, float amount, string desc, string currency)
            : base(messageID, requesterID, chatID, timestamp)
        {
            this.MessageID = messageID;
            this.SenderID = requesterID;
            this.ChatID = chatID;
            this.Timestamp = timestamp;
            this.Status = status;
            this.Amount = amount;
            this.Description = desc;
            this.Currency = currency;
        }

        /// <summary>
        /// Gets or sets the amount associated with the request.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Gets or sets the description of the request.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the currency of the amount.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets the formatted amount with currency.
        /// </summary>
        public string FormattedAmount => $"{this.Amount} {this.Currency}";

        private int MessageID { get; set; }

        private int SenderID { get; set; }

        private int ChatID { get; set; }

        private DateTime Timestamp { get; set; }

        private string Status { get; set; }

        /// <summary>
        /// Gets the unique identifier of the message.
        /// </summary>
        /// <returns>The Message ID.</returns>
        public new int GetMessageID() => this.MessageID;

        /// <summary>
        /// Gets the unique identifier of the sender.
        /// </summary>
        /// <returns>The sender ID.</returns>
        public new int GetSenderID() => this.SenderID;

        /// <summary>
        /// Gets the unique identifier of the chat.
        /// </summary>
        /// <returns>The Chat ID.</returns>
        public new int GetChatID() => this.ChatID;

        /// <summary>
        /// Gets the timestamp of the message.
        /// </summary>
        /// <returns>The Timestamp.</returns>
        public new DateTime GetTimestamp() => this.Timestamp;

        /// <summary>
        /// Gets the status of the request.
        /// </summary>
        /// <returns>The Status.</returns>
        public string GetStatus() => this.Status;

        /// <summary>
        /// Gets the amount associated with the request.
        /// </summary>
        /// <returns>The Amount.</returns>
        public float GetAmount() => this.Amount;

        /// <summary>
        /// Gets the description of the request.
        /// </summary>
        /// <returns>The Description.</returns>
        public string GetDescription() => this.Description;

        /// <summary>
        /// Gets the currency of the amount.
        /// </summary>
        /// <returns>The Currency.</returns>
        public string GetCurrency() => this.Currency;

        /// <summary>
        /// Returns a string representation of the transfer message.
        /// </summary>
        /// <returns>A string describing the transfer message.</returns>
        public override string ToString()
        {
            return $"Request Message: {this.Amount}{this.Currency} - {this.Description}";
        }
    }
}