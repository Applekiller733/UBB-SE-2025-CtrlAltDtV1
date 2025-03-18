using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model
{
    class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public List<int> Friends { get; set; }
        public List<int> Chats { get; set; }
        public int ReportedCount { get; set; }

        public User() { }
        public User(int userId, string username, string phoneNumber, List<int> friends, List<int> chats, int reportedCount)
        {
            UserId = userId;
            Username = username;
            PhoneNumber = phoneNumber;
            Friends = friends;
            Chats = chats;
            ReportedCount = reportedCount;
        }
        public User(int userId, string username, string phoneNumber, List<int> friends, List<int> chats)
        {
            UserId = userId;
            Username = username;
            PhoneNumber = phoneNumber;
            Friends = friends;
            Chats = chats;
            ReportedCount = 0;
        }


        public override string ToString()
        {
            return $"User ID: {UserId}, " +
                $"Username: {Username}, " +
                $"Phone Number: {PhoneNumber}, " +
                $"Friends: {string.Join(", ", Friends)}, " +
                $"Chat IDs: {string.Join(", ", Chats)}, " +
                $"Reported Count: {ReportedCount}";
        }

        public void AddFriend(int friendID)
        {
            Friends.Add(friendID);
        }

        public void RemoveFriend(int friendID)
        {
            Friends.Remove(friendID);
        }

        public void JoinChat(int chat)
        {
            Chats.Add(chat);
        }

        public void LeaveChat(int chat)
        {
            Chats.Remove(chat);
        }

        public int GetFriendCount()
        {
            return Friends.Count;
        }

        public int GetChatCount()
        {
            return Chats.Count;
        }

        public void IncreaseReportCount()
        {
            ReportedCount++;
        }

        public void resetReportCount()
        {
            ReportedCount = 0;
        }
    }
}
