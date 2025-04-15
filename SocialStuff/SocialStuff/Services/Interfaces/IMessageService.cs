using SocialStuff.Data;
using SocialStuff.Model.MessageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Interfaces
{
    public interface IMessageService
    {
        IRepository getRepo();
        void sendMessage(int SenderID, int ChatID, string Content);
        void sendImage(int SenderID, int ChatID, string ImageURL);
        void deleteMessage(Message message);
        void sendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency);
        void sendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency);
    }
}
