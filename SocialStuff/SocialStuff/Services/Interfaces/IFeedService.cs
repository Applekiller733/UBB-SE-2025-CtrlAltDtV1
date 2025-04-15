using SocialStuff.Data;
using SocialStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Interfaces
{
    public interface IFeedService
    {
        List<Post> GetFeedContent();
    }
}
