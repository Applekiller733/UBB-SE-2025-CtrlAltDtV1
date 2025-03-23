using SocialStuff.Data;
using SocialStuff.Model;
using System.Collections.Generic;

namespace SocialStuff.Services
{
    public class FeedService
    {
        private readonly Repository repository;
        private readonly UserService userService;

        public FeedService(Repository repo, UserService userService)
        {
            this.repository = repo;
            this.userService = userService;
        }

        public List<Post> GetFeedContent()
        {
            return repository.GetFeedPostsList();
        }
    }
}