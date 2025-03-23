using System;

namespace SocialStuff.Model
{
    public class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Post(int postID, string title, string category, string content, DateTime timestamp)
        {
            PostID = postID;
            Title = title;
            Category = category;
            Content = content;
            Timestamp = timestamp;
        }
    }
}