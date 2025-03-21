using SocialStuff.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Model;
using Microsoft.Identity.Client;

namespace SocialStuff.Services
{
    public class ChatService
    {
        private Repository repository;
        public const int chatID = 1;

        //Get current chat ID
        public int getCurrentChatID()
        {
            return chatID;
        }
        //Get logged in user ID
        public int GetCurrentUserID()
        {
            return repository.GetLoggedInUserID();
        }

        //return the number of participants in a chat beside the user
        public int getNumberOfParticipants(int ChatID)
        {
            return repository.GetChatParticipantsIDs(getCurrentChatID()).Count - 1;
        }

        public ChatService(Repository repo)
        {
            this.repository = repo;
        }

        public Repository getRepo()
        {
            return this.repository;
        }




            /// MODIFY ERROR REQUEST HAS 1 INSTANCE WITH REQUESTER ID AND CHAT 1



        //Creates a new requestmessage and adds it to the database
        public void requestMoneyViaChat(float Amount, string Currency, int ChatID, string Description)
        {
            if (Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(Currency))
            {
                throw new ArgumentException("Currency cannot be null or empty.");
            }

           

            List<int> participantIDs = repository.GetChatParticipantsIDs(ChatID);
            int currentUserId = GetCurrentUserID();

            foreach (int participantID in participantIDs)
            {
                if (currentUserId != participantID)
                {
                    try
                    {
                        repository.AddRequestMessage(currentUserId, participantID, Description, "Pending", Amount, Currency);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"Error requesting money from participant {participantID}: {ex.Message}");
                    }
                }
            }
        }





        ///ERROR ONLY 1 INSTANCE 1 SENDER ID 1 GROUP ID

        //creates a new transfermessage and adds it to the database
        public void sendMoneyViaChat(float Amount, string Currency, string Description, int ChatID)
        {
            if (Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(Currency))
            {
                throw new ArgumentException("Currency cannot be null or empty.");
            }


            List<int> participantIDs = repository.GetChatParticipantsIDs(ChatID);
            int currentUserId = GetCurrentUserID();

            foreach (int participantID in participantIDs)
            {
                if (currentUserId != participantID)
                {
                    try
                    {
                        repository.AddTransferMessage(currentUserId, participantID, Description, "", Amount, Currency);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"Error sending money to participant {participantID}: {ex.Message}");
                    }
                }
            }
        }

        


        public void createChat(List<int> ParticipantsID, string ChatName)
        {
            throw new NotImplementedException();
        }

        public void deleteChat(int ChatID)
        {
            this.repository.DeleteChat(ChatID);
        }

        public DateTime getLastMessageTimeStamp(int ChatID)
        {
            List<Message> allMessages = this.repository.GetMessagesList();

            var chatMessages = allMessages.Where(m => m.getChatID() == ChatID).ToList();

            var lastMessage = chatMessages.OrderByDescending(m => m.getTimestamp()).FirstOrDefault();

            if (lastMessage == null)
            {
                throw new Exception("No messages to show in the chat with id: " + ChatID);
            }

            return lastMessage.getTimestamp();
        }

        public List<Message> getChatHistory(int ChatID)
        {
            List<Message> allMessages = this.repository.GetMessagesList();

            List<Message> chatHistory = allMessages.Where(m => m.getChatID() == ChatID).ToList();

            return chatHistory;
        }

        public void AddUserToChat(int UserID, int ChatID)
        {
            this.repository.AddUserToChat(UserID, ChatID);
        }

        public void RemoveUserFromChat(int UserID, int ChatID)
        {
            this.repository.RemoveUserFromChat(UserID, ChatID);
        }
    }
}
