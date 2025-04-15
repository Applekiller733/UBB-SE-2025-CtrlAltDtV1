// <copyright file="FeedService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Services.Implementations
{
    using System.Collections.Generic;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    /// <summary>
    /// Service for managing and retrieving feed content.
    /// </summary>
    public class FeedService : IFeedService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedService"/> class.
        /// </summary>
        /// <param name="repo">The repository instance for data access.</param>
        /// <param name="userService">The user service instance for user-related operations.</param>
        public FeedService(IRepository repo, IUserService userService)
        {
            this.repository = repo;
            this.userService = userService;
        }

        /// <summary>
        /// Retrieves the list of posts for the feed.
        /// </summary>
        /// <returns>A list of <see cref="Post"/> objects representing the feed content.</returns>
        public List<Post> GetFeedContent()
        {
            return this.repository.GetFeedPostsList();
        }
    }
}
