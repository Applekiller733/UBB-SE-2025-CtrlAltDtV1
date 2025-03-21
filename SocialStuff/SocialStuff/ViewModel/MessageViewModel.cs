using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.ViewModel
{
    public class MessageViewModel
    {
        public int SenderID { get; set; }
        public int ChatID { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsImage { get; set; }
        public string ImageUrl { get; set; }

        public string FormattedTime => Timestamp.ToString("HH:mm"); // Formatting for UI
        public bool IsOwnMessage => SenderID == 1; // Replace with actual user ID check
    }
}
