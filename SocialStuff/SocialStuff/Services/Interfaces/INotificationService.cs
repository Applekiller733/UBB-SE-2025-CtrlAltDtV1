using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationModel = SocialStuff.Model.Notification;

namespace SocialStuff.Services.Interfaces
{
    public interface INotificationService
    {
        List<NotificationModel> GetNotifications(int userID);  //notifications for a specific user
        void SendFriendNotification(int userID, int newFriendID);
        void SendRemoveFriendNotification(int userID, int oldFriendID);
        void SendMessageNotification(int messageSenderID, int chatID);
        void SendTransactionNotification(int receiverID, int chatID, string type, float amount, string currency);
        void SendNewChatNotification(int chatID);
        void ClearNotification(int notificationID);
        void ClearAllNotifications(int userID);
    }
}
