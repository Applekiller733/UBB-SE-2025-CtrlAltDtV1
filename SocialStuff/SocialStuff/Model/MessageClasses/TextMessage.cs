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
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ChatID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public List<int> UsersReport { get; set; }

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

        public override string ToString() { return Content; }
    }
}