using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Model.MessageClasses
{
    public class TransferMessage : Message
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ChatID { get; set; }
        public string Status { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public List<int> ListOfReceiversID { get; set; }

        public TransferMessage(int messageID, int senderID, int chatID, string status, float amount, string desc, string currency)
            : base(senderID, chatID)
        {
            MessageID = messageID;
            Status = status;
            Amount = amount;
            Description = desc;
            Currency = currency;
            ListOfReceiversID = new List<int>();
        }

        public TransferMessage(int messageID, int senderID, int chatID, DateTime timestamp, string status, float amount, string desc, string currency)
            : base(senderID, chatID, timestamp)
        {
            MessageID = messageID;
            Status = status;
            Amount = amount;
            Description = desc;
            Currency = currency;
            ListOfReceiversID = new List<int>();
        }

        public override string ToString()
        {
            return $"Transfer Message: {Amount}{Currency} - {Description}";
        }
    }
}