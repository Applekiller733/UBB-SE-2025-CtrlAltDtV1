using Microsoft.Data.SqlClient;
using SocialStuff.Model;
using SocialStuff.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocialStuff.Services
{
    internal class UserService
    {

        private Repository repo; //readonly?
        private int UserID;

        public UserService(Repository repo)
        {
            repo = repo;
            UserID = GetCurrentUser();
        }

        public Repository GetRepo()
        {
            return repo;
        }

        public void AddFriend(int userID, int newFriendID)
        {
            var user = GetUserById(userID);
            var friend = GetUserById(newFriendID);
            var friends = repo.GetFriendsIDs(userID); // to implement

            if (user != null && friend != null && !friends.Contains(newFriendID))
            {
                // friends is one way process so we add the friend to the user's friend list
                repo.AddFriend(userID, newFriendID);  //function from repo
                user.AddFriend(newFriendID);

                //friend.AddFriend(userID);
                //repo.UpdateUser(friend);
            }
        }

        public void RemoveFriend(int userID, int oldFriendID)
        {
            var user = GetUserById(userID);
            var friend = GetUserById(oldFriendID);
            var friends = repo.GetFriendsIDs(userID); // to implement

            if (user != null && friend != null && friends.Contains(oldFriendID))
            {
                repo.DeleteFriend(userID, oldFriendID); //function from repo
                user.RemoveFriend(oldFriendID);

                //friend.RemoveFriend(userID);
                //repo.UpdateUser(friend);
            }
        }

        public void JoinChat(int userID, int chatID)
        {
            var user = GetUserById(userID);
            var chats = repo.GetChatsIDs(userID); // to implement

            if (user != null && !chats.Contains(chatID))
            {
                repo.AddUserToChat(chatID, userID);
                user.JoinChat(chatID);
            }
        }

        public void LeaveChat(int userID, int chatID)
        {
            var user = GetUserById(userID);
            var chats = repo.GetChatsIDs(userID); // to implement

            if (user != null && chats.Contains(chatID))
            {
                repo.RemoveUserFromChat(chatID, userID);
                user.LeaveChat(chatID);
            }
        }

        public List<int> FilterUsers(string keyword, int userID)
        {
            var users = repo.GetUsersList();
            return users.Where(u => (u.GetUsername().Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                    || u.GetPhoneNumber().Contains(keyword)) && u.GetUserId() != userID)
                        .Select(u => u.GetUserId())
                        .ToList();
        }

        public List<int> FilterFriends(string keyword, int userID)
        {
            var user = GetUserById(userID);
            if (user == null) return new List<int>();
            var friends = repo.GetFriends(userID); // to implement

            return friends
                       .Select(friendID => friendID)
                       .Where(friend => friend != null &&
                                        (friend.GetUsername().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         friend.GetPhoneNumber().Contains(keyword)))
                       .Select(friend => friend.GetUserId())
                       .ToList();
        }

        public List<int> GetFriendsByUser(int userID)
        {
            var user =  GetUserById(userID);
            var friends = repo.GetFriendsIDs(userID); // to implement

            return friends;
        }

        public User GetUserById(int userID)
        {
            List<User> users = repo.GetUsersList();
            foreach (User user in users) 
            {
                if (user.GetUserId() == userID)
                {
                    return user;
                }
            }

            return null; 
        }

        public int GetCurrentUser()
        {
            return repo.GetLoggedInUserID(); // This should be replaced with actual logic to get the logged-in user.
        }
    }
}
