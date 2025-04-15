using SocialStuff.Data;
using SocialStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.Services.Interfaces
{
    public interface IUserService
    {
        IRepository GetRepo();
        void AddFriend(int userID, int newFriendID);
        void RemoveFriend(int userID, int oldFriendID);
        void JoinChat(int userID, int chatID);
        void LeaveChat(int userID, int chatID);
        List<int> FilterUsers(string keyword, int userID);
        List<int> FilterFriends(string keyword, int userID);
        List<int> GetFriendsIDsByUser(int userID);
        List<User> GetFriendsByUser(int userID);
        List<int> GetChatsByUser(int userID);
        List<Chat> GetCurrentUserChats();
        User GetUserById(int userID);
        List<User> GetNonFriendsUsers(int UserID);
        int GetCurrentUser();   // This should be replaced with actual logic to get the logged-in user. ---> JUST A MOCK FUNCTION
        void MarkUserAsDangerousAndGiveTimeout(User user);
        bool IsUserInTimeout(User user);
        void setUserTimeout(bool value);
    }
}
