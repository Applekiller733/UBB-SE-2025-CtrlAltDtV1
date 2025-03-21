using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public class ImageMessage : Message
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ChatID { get; set; }
        public DateTime Timestamp { get; set; }

        public string ImageURL { get; set; }
        public List<int> UsersReport { get; set; }

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

        public override string ToString()
        {
            return ImageURL;
        }
    }
}