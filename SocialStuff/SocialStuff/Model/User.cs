// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.Username = string.Empty; // Initialize to a default non-null value
            this.PhoneNumber = string.Empty; // Initialize to a default non-null value
            this.Friends = new List<int>(); // Initialize to an empty list
            this.Chats = new List<int>(); // Initialize to an empty list
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier for the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="reportedCount">The number of times the user has been reported.</param>
        public User(int userId, string username, string phoneNumber, int reportedCount)
        {
            this.UserId = userId;
            this.Username = username;
            this.PhoneNumber = phoneNumber;
            this.ReportedCount = reportedCount;
            this.Friends = new List<int>();
            this.Chats = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier for the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="friends">The list of friend IDs associated with the user.</param>
        /// <param name="chats">The list of chat IDs associated with the user.</param>
        public User(int userId, string username, string phoneNumber, List<int> friends, List<int> chats)
        {
            this.UserId = userId;
            this.Username = username;
            this.PhoneNumber = phoneNumber;
            this.Friends = friends;
            this.Chats = chats;
            this.ReportedCount = 0;
        }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <returns>The user ID.</returns>
        private int UserId { get; set; }

        /// <summary>
        /// Gets or sets the number of times the user has been reported.
        /// </summary>
        /// <returns>The user ID.</returns>
        private int ReportedCount { get; set; }

        /// <summary>
        /// Gets or sets the list of friend ID's associated with the user.
        /// </summary>
        /// <returns>The user ID.</returns>
        private List<int> Friends { get; set; }

        /// <summary>
        /// Gets or sets the list of chat ID's associated with the user.
        /// </summary>
        /// <returns>The user ID.</returns>
        private List<int> Chats { get; set; }

        /// <summary>
        /// Gets or sets the timeout end time of the user.
        /// </summary>
        /// <returns>The user ID.</returns>
        private DateTime? TimeoutEnd { get; set; }

        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        /// <returns>The user ID.</returns>
        public int GetUserId()
        {
            return this.UserId;
        }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        /// <returns>The username.</returns>
        public string GetUsername()
        {
            return this.Username;
        }

        /// <summary>
        /// Gets the phone number of the user.
        /// </summary>
        /// <returns>The phone number.</returns>
        public string GetPhoneNumber()
        {
            return this.PhoneNumber;
        }

        /// <summary>
        /// Gets the number of times the user has been reported.
        /// </summary>
        /// <returns>The reported count.</returns>
        public int GetReportedCount()
        {
            return this.ReportedCount;
        }

        /// <summary>
        /// Gets the list of friend IDs associated with the user.
        /// </summary>
        /// <returns>The list of friend IDs.</returns>
        public List<int> GetFriends()
        {
            return this.Friends;
        }

        /// <summary>
        /// Gets the list of chat IDs associated with the user.
        /// </summary>
        /// <returns>The list of chat IDs.</returns>
        public List<int> GetChats()
        {
            return this.Chats;
        }

        /// <summary>
        /// Gets the timeout end time for the user.
        /// </summary>
        /// <returns>The timeout end time, or null if no timeout is set.</returns>
        public DateTime? GetTimeoutEnd() => this.TimeoutEnd;

        /// <summary>
        /// Sets the timeout end time for the user.
        /// </summary>
        /// <param name="timeoutEnd">The timeout end time to set.</param>
        public void SetTimeoutEnd(DateTime? timeoutEnd) => this.TimeoutEnd = timeoutEnd;

        /// <summary>
        /// Returns a string representation of the user.
        /// </summary>
        /// <returns>A string containing user details.</returns>
        public override string ToString()
        {
            return $"User ID: {this.UserId}, " +
                $"Username: {this.Username}, " +
                $"Phone Number: {this.PhoneNumber}, " +
                $"Reported Count: {this.ReportedCount}" +
                $"Friends: {this.Friends}, " +
                $"Chats: {this.Chats}";
        }

        /// <summary>
        /// Increases the report count for the user and applies a timeout if necessary.
        /// </summary>
        /// <returns>A message indicating the user's timeout status.</returns>
        public string IncreaseReportCount()
        {
            this.ReportedCount++;
            System.Diagnostics.Debug.WriteLine($"User {this.Username} report count increased to {this.ReportedCount}");

            string message = string.Empty;
            if (this.ReportedCount >= 1)
            {
                this.SetTimeoutEnd(DateTime.Now.AddMinutes(3));
                System.Diagnostics.Debug.WriteLine($"User {this.Username} set in timeout until {this.TimeoutEnd}");
                this.ResetReportCountAfterDelay();

                message = $"User {this.Username} has been reported {this.ReportedCount} times and is now in timeout until {this.TimeoutEnd?.ToString("HH:mm:ss")}";
            }

            return message;
        }

        /// <summary>
        /// Resets the report count for the user.
        /// </summary>
        public void ResetReportCount()
        {
            this.ReportedCount = 0;
        }

        /// <summary>
        /// Adds a friend to the user's friend list.
        /// </summary>
        /// <param name="friendID">The ID of the friend to add.</param>
        public void AddFriend(int friendID)
        {
            this.Friends.Add(friendID);
        }

        /// <summary>
        /// Removes a friend from the user's friend list.
        /// </summary>
        /// <param name="friendID">The ID of the friend to remove.</param>
        public void RemoveFriend(int friendID)
        {
            this.Friends.Remove(friendID);
        }

        /// <summary>
        /// Adds the user to a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat to join.</param>
        public void JoinChat(int chatID)
        {
            this.Chats.Add(chatID);
        }

        /// <summary>
        /// Removes the user from a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat to leave.</param>
        public void LeaveChat(int chatID)
        {
            this.Chats.Remove(chatID);
        }

        /// <summary>
        /// Resets the report count after a delay.
        /// </summary>
        private void ResetReportCountAfterDelay()
        {
            Timer timer = new Timer(3 * 60 * 1000); // 3 minutes in milliseconds
            timer.Elapsed += (sender, e) =>
            {
                this.ResetReportCount();
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}