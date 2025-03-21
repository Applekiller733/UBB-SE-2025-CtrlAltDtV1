using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace SocialStuff.Model.MessageClasses
{
    public class TextMessage : Message
    {
        private int MessageID;
        private int SenderID;
        private int ChatID;
        private DateTime Timestamp;
        private string Content;
        private List<int> UsersReport;
        public TextMessage(int messageID, int senderID, int chatID, string content)
           : base(senderID, chatID)
        {
            this.MessageID = messageID;
            this.Content = content;
            this.UsersReport = new List<int>();
        }

        public TextMessage(int messageID, int senderID, int chatID, DateTime timestamp, string content, List<int> usersReport)
            : base(senderID, chatID, timestamp)
        {
            this.MessageID = messageID;
            this.Content = content;
            this.UsersReport = usersReport;
        }

        public int getChatID() { return ChatID; }

        public int getSenderID() { return SenderID; }

        public DateTime getTimestamp() { return Timestamp; }

        public string getTextContent() { return Content; }

        public override string ToString() { return Content; }
    }
}