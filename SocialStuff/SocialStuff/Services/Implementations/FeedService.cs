using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Services.Interfaces;
using System.Collections.Generic;

namespace SocialStuff.Services.Implementations
{
    public class FeedService : IFeedService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;

        public FeedService(IRepository repo, IUserService userService)
        {
            repository = repo;
            this.userService = userService;
        }

        public List<Post> GetFeedContent()
        {
            return repository.GetFeedPostsList();
        }
    }
}
