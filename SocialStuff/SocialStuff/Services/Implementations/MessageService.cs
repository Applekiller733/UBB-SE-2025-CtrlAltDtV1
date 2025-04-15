using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private IRepository repository;
        private IUserService userService;

        public MessageService(IRepository repo, IUserService userService)
        {
            repository = repo;
            this.userService = userService;
        }

        public IRepository getRepo()
        {
            return repository;
        }

          // Check if user is in timeout
        private bool IsUserInTimeout(int userID)
        {
            //i think i solved the dependency???
            User user = userService.GetUserById(userID);
            bool isInTimeout = user != null && userService.IsUserInTimeout(user);
            System.Diagnostics.Debug.WriteLine($"MessageService checking if user {user?.GetUsername()} is in timeout: {isInTimeout}");
            return isInTimeout;
        }

        //for all the functionalities below, we checked if the user is in timeout
        public void sendMessage(int SenderID, int ChatID, string Content)
        {
            if (IsUserInTimeout(SenderID)) return;
            //if(UserService.IsUserInTimeout()) return;
            repository.AddTextMessage(SenderID, ChatID, Content);
        }

        public void sendImage(int SenderID, int ChatID, string ImageURL)
        {
            if (IsUserInTimeout(SenderID)) return;
            repository.AddImageMessage(SenderID, ChatID, ImageURL);
        }


        public void deleteMessage(Message message)
        {
            repository.DeleteMessage(message.getMessageID());
        }
        public void sendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            repository.AddTransferMessage(userID, chatID, content, status, amount, currency);
        }

        public void sendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            repository.AddRequestMessage(userID, chatID, content, status, amount, currency);
        }

        internal void reportMessage(Message message)
        {
            //this.repository.(message.getMessageID());
        }
    }
}