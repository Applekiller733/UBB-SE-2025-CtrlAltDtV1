using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public class RequestMessage : Message
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ChatID { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        public RequestMessage(int messageID, int requesterID, int chatID, string status, float amount, string desc, string currency)
            : base(requesterID, chatID)
        {
            MessageID = messageID;
            Status = status;
            Amount = amount;
            Description = desc;
            Currency = currency;
        }

        public RequestMessage(int messageID, int requesterID, int chatID, DateTime timestamp, string status, float amount, string desc, string currency)
            : base(requesterID, chatID, timestamp)
        {
            MessageID = messageID;
            Status = status;
            Amount = amount;
            Description = desc;
            Currency = currency;
        }

        public override string ToString()
        {
            return $"Request Message: {Amount}{Currency} - {Description}";
        }
    }
}