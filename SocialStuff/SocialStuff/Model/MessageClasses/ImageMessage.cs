using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public class ImageMessage : Message
    {
        private int MessageID;
        private int SenderID;
        private int ChatID;
        private DateTime Timestamp;
        private string ImageURL;
        private List<int> UsersReport;

        public ImageMessage(int messageID, int senderID, int chatID, string imageUrl, List<int> usersReport)
            : base(senderID, chatID)
        {
            MessageID = messageID;
            ImageURL = imageUrl;
            UsersReport = usersReport;
        }

        public ImageMessage(int messageID, int senderID, int chatID, DateTime timestamp, string imageUrl, List<int> usersReport)
            : base(senderID, chatID, timestamp)
        {
            MessageID = messageID;
            ImageURL = imageUrl;
            UsersReport = usersReport;
        }

        public string getImageURL() { return ImageURL; }
        public int getSenderID() { return SenderID; }

        public int getChatID() { return ChatID; }
        public DateTime getTimestamp() { return Timestamp; }
        public override string ToString() { return ImageURL; }
    }
}