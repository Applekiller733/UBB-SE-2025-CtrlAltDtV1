using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public abstract class Message
    {
        private int MessageID { get; set; }
        private int SenderID { get; set; }
        private int ChatID { get; set; }
        private DateTime Timestamp { get; set; }

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

        public override string ToString()
        {
            return $"{GetType().Name} from {SenderID} in chat {ChatID} at {Timestamp}";
        }
    }
}