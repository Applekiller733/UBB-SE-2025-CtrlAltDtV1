using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public abstract class Message
    {
        public int SenderID;
        public int ChatID;
        public DateTime Timestamp;

        protected Message(int senderID, int chatID)
        {
            SenderID = senderID;
            ChatID = chatID;
            Timestamp = DateTime.Now;
        }

        protected Message(int senderID, int chatID, DateTime timestamp)
        {
            SenderID = senderID;
            ChatID = chatID;
            Timestamp = timestamp;
        }

        public int getSenderID()
        {
            return this.SenderID;
        }

        public int getChatID()
        {
            return this.ChatID;
        }

        public DateTime getTimestamp()
        {
            return this.Timestamp;
        }

        public override string ToString()
        {
            return $"{GetType().Name} from {SenderID} in chat {ChatID} at {Timestamp}";
        }
    }
}