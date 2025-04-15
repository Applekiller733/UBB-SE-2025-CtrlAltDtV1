// <copyright file="IFeedService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SocialStuff.Data;
    using SocialStuff.Model;

    /// <summary>
    /// Provides methods to retrieve feed content.
    /// </summary>
    public interface IFeedService
    {
        /// <summary>
        /// Retrieves the content of the feed.
        /// </summary>
        /// <returns>A list of posts representing the feed content.</returns>
        List<Post> GetFeedContent();
    }
}
