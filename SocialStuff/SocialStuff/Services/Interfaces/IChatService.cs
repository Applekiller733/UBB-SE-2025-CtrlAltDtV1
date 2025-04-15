using SocialStuff.Data;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Interfaces
{
    public interface IChatService
    {
        int GetCurrentUserID();
        int getNumberOfParticipants(int ChatID);
        IRepository getRepo();
        void requestMoneyViaChat(float Amount, string Currency, int ChatID, string Description);
        void sendMoneyViaChat(float Amount, string Currency, string Description, int ChatID);
        void acceptRequestViaChat(float amount, string currency, int AccepterID, int RequesterID, int ChatID);
        //mock enough funds function
        bool enoughFunds(float amount, string currency, int SenderID);
        //mock transfer function
        void initiateTransfer(int senderID, int reciverID, float amount, string currency);
        void createChat(List<int> ParticipantsID, string ChatName);
        void deleteChat(int ChatID);
        DateTime getLastMessageTimeStamp(int ChatID);
        List<Message> getChatHistory(int ChatID);
        void AddUserToChat(int UserID, int ChatID);
        void RemoveUserFromChat(int UserID, int ChatID);
        string getChatNameByID(int ChatID);
        List<string> getChatParticipantsStringList(int ChatID);
        List<User> getChatParticipantsList(int ChatID);
    }
}
