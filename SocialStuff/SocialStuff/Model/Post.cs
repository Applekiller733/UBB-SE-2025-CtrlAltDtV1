// <copyright file="Post.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Model
{
    using System;

    /// <summary>
    /// Represents a post with an ID, title, category, content, and timestamp.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Post"/> class.
        /// </summary>
        /// <param name="postID">The unique identifier for the post.</param>
        /// <param name="title">The title of the post.</param>
        /// <param name="category">The category of the post.</param>
        /// <param name="content">The content of the post.</param>
        /// <param name="timestamp">The timestamp when the post was created.</param>
        public Post(int postID, string title, string category, string content, DateTime timestamp)
        {
            this.PostID = postID;
            this.Title = title;
            this.Category = category;
            this.Content = content;
            this.Timestamp = timestamp;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the post.
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the category of the post.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the post was created.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}