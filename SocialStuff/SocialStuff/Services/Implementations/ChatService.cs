using SocialStuff.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Model;
using Microsoft.Identity.Client;
using SocialStuff.Services.Interfaces;

namespace SocialStuff.Services.Implementations
{
    public class ChatService : IChatService
    {
        private IRepository repository;
        public const int chatID = 1;


        //Get logged in user ID
        public int GetCurrentUserID()
        {
            return repository.GetLoggedInUserID();
        }

        //return the number of participants in a chat beside the user
        public int getNumberOfParticipants(int ChatID)
        {
            return repository.GetChatParticipantsIDs(ChatID).Count;
        }

        public ChatService(IRepository repo)
        {
            repository = repo;
        }

        public IRepository getRepo()
        {
            return repository;
        }




            /// MODIFY ERROR REQUEST HAS 1 INSTANCE WITH REQUESTER ID AND CHAT 1 -: DONE (depends on implementation)



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

            try
            {
                repository.AddRequestMessage(GetCurrentUserID(), ChatID, Description, "Pending", Amount, Currency);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error requesting money from participant {ChatID}: {ex.Message}");
            }
        }





      

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

            try
            {
                if (enoughFunds(Amount*(participantIDs.Count-1), Currency, GetCurrentUserID()))
                {
                    int currentUserId = GetCurrentUserID();
                   
                    foreach (int reciverid in participantIDs)
                    {
                        if (currentUserId != reciverid)
                        {
                            initiateTransfer(currentUserId, reciverid, Amount, Currency);

                        }
                    }
                    repository.AddTransferMessage(GetCurrentUserID(), ChatID, Description, "Accepted", Amount * (participantIDs.Count - 1), Currency);


                }
                else
                {
                    repository.AddTransferMessage(GetCurrentUserID(), ChatID, Description, "Rejected", Amount * (participantIDs.Count - 1), Currency);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending money to participant {ChatID}: {ex.Message}");
            }


        }

        //when a reqest is initiated in chat if you press accept a new transfer message is initiated with sender id the one who pressed and 
        //chat id the chat 
        public void acceptRequestViaChat(float amount, string currency, int AccepterID, int RequesterID, int ChatID)
        {
            //from Adrada
            if (enoughFunds(amount,currency, AccepterID))
            {
                initiateTransfer(AccepterID, RequesterID, amount, currency);
                repository.AddTransferMessage(AccepterID, ChatID, "YOU JUST SENT " + amount.ToString() + currency +
                    "TO here function that retrives username by ID", "Accepted", amount, currency);     
            }
            else
            {
                repository.AddTransferMessage(AccepterID, ChatID, "YOU FAILED TO SEND " + amount.ToString() + currency +
                    "TO here function that retrives username by ID", "Rejected", amount, currency);
            }
            
        }

        //mock function for enough funds
        public bool enoughFunds(float amount, string currency, int SenderID)
        {

            Random random = new Random();
            bool randomBool = random.Next(2) == 0; 
            return randomBool;
            
        }
        //mock transfer function --- THAT'S WHY IT DOESN'T WORK
        public void initiateTransfer(int senderID, int reciverID, float amount, string currency)
        {
            return;
        }

        


        public void createChat(List<int> ParticipantsID, string ChatName)
        {

            int chatID = repository.AddChat(ChatName);

            foreach (var userID in ParticipantsID)
            {
                repository.AddUserToChat(userID, chatID);
            }
            var addedUsers = repository.GetChatParticipants(chatID);

        }

        public void deleteChat(int ChatID)
        {
            repository.DeleteChat(ChatID);
        }

        public DateTime getLastMessageTimeStamp(int ChatID)
        {
            List<Message> allMessages = repository.GetMessagesList();

            var chatMessages = allMessages.Where(m => m.getChatID() == ChatID).ToList();

            var lastMessage = chatMessages.OrderByDescending(m => m.getTimestamp()).FirstOrDefault();

            if (lastMessage == null)
            {
                // return a lower datetime
                return DateTime.MinValue;
            }

            return lastMessage.getTimestamp();
        }

        public List<Message> getChatHistory(int ChatID)
        {
            List<Message> allMessages = repository.GetMessagesList();

            List<Message> chatHistory = allMessages.Where(m => m.getChatID() == ChatID).ToList();

            return chatHistory;
        }

        public void AddUserToChat(int UserID, int ChatID)
        {
            repository.AddUserToChat(UserID, ChatID);
        }

        public void RemoveUserFromChat(int UserID, int ChatID)
        {
            repository.RemoveUserFromChat(UserID, ChatID);
        }

        public string getChatNameByID(int ChatID)
        {
            List<Chat> chatList = repository.GetChatsList();
            string chatName = chatList.Where(c => c.getChatID() == ChatID).FirstOrDefault().getChatName();

            return chatName;
        }

        public List<string> getChatParticipantsStringList(int ChatID)
        {
            List<User> participants = repository.GetChatParticipants(ChatID);
            List<string> participantsList = participants.Select(p => p.GetUsername()).ToList();
            return participantsList;
        }

        public List<User> getChatParticipantsList(int ChatID)
        {
            List<User> participants = repository.GetChatParticipants(ChatID);
            return participants;
        }
    }
}
