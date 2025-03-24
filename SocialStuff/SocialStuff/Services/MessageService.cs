using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services
{
    public class MessageService
    {
        private Repository repository;

        public MessageService(Repository repo)
        {
            this.repository = repo;
        }

        public Repository getRepo()
        {
            return this.repository;
        }

          // Check if user is in timeout
        private bool IsUserInTimeout(int userID)
        {

            UserService userService = new UserService(repository);
            User user = userService.GetUserById(userID);
            bool isInTimeout = user != null && userService.IsUserInTimeout(user);
            System.Diagnostics.Debug.WriteLine($"MessageService checking if user {user?.GetUsername()} is in timeout: {isInTimeout}");
            return isInTimeout;
        }

        //for all the functionalities below, we checked if the user is in timeout
        public void sendMessage(int SenderID, int ChatID, string Content)
        {
            if (this.IsUserInTimeout(SenderID)) return;
            if(UserService.IsUserInTimeout()) return;
            this.repository.AddTextMessage(SenderID, ChatID, Content);
        }

        public void sendImage(int SenderID, int ChatID, string ImageURL)
        {
            if (this.IsUserInTimeout(SenderID)) return;
            this.repository.AddImageMessage(SenderID, ChatID, ImageURL);
        }


        public void deleteMessage(Message message)
        {
            this.repository.DeleteMessage(message.getMessageID());
        }
        public void sendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            this.repository.AddTransferMessage(userID, chatID, content, status, amount, currency);
        }

        public void sendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            this.repository.AddRequestMessage(userID, chatID, content, status, amount, currency);
        }

        internal void reportMessage(Message message)
        {
            //this.repository.(message.getMessageID());
        }
    }
}