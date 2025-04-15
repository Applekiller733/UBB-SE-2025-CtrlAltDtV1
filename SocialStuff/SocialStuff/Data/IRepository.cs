using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModel = SocialStuff.Model.Notification;

namespace SocialStuff.Data
{
    public interface IRepository
    {
        //user part
        int GetLoggedInUserID();
        User GetUserById(int userId);
        List<User> GetUsersList();
        List<User> GetUserFriendsList(int userId);
        List<int> GetFriendsIDs(int userId);
        void AddFriend(int userId, int friendId);
        void DeleteFriend(int userId, int friendId);
        
        //chat part
        Chat? GetChatById(int chatId);
        List<Chat> GetChatsList();
        List<User> GetChatParticipants(int chatId);
        List<int> GetChatParticipantsIDs(int chatId);
        List<int> GetChatsIDs(int userId);
        int AddChat(string chatName);
        void DeleteChat(int chatId);
        void AddUserToChat(int userId, int chatId);
        void RemoveUserFromChat(int userId, int chatId);
        
        //message part
        List<Message> GetMessagesList();
        void AddTextMessage(int userId, int chatId, string content);
        void AddImageMessage(int userId, int chatId, string imageURL);
        void AddRequestMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null);
        void AddTransferMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null);
        void DeleteMessage(int messageId);
        
        //notification part
        List<NotificationModel> GetNotifications(int userId);
        void AddNotification(string content, int userId);
        void DeleteNotification(int notificationId);
        void ClearAllNotifications(int userId);
       
        //report part
        List<Report> GetReportsList();
        void AddReport(int messageId, string reason, string description, string status);


        //feed part
        List<Post> GetFeedPostsList();
    }
}
