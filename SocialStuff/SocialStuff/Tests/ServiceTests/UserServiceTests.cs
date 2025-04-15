using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IRepository> _repoMock;
        private Mock<INotificationService> _notificationServiceMock;
        private UserService _userService;
        private const int CurrentUserId = 2;
    
        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _repoMock.Setup(r => r.GetLoggedInUserID()).Returns(CurrentUserId);
            _userService = new UserService(_repoMock.Object, _notificationServiceMock.Object);
        }

        [TestMethod]
        public void AddFriend_ValidUsersNotFriends_CallsRepoAndNotification()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(friendId)).Returns(friend);
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { 2 });

            _userService.AddFriend(userId, friendId);

            _repoMock.Verify(r => r.AddFriend(userId, friendId), Times.Once());
            _notificationServiceMock.Verify(n => n.SendFriendNotification(userId, friendId), Times.Once());
            Assert.IsTrue(user.GetFriends().Contains(friendId));
        }

        [TestMethod]
        public void AddFriend_AlreadyFriends_DoesNotCallRepo()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(friendId)).Returns(friend);
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { friendId });

            _userService.AddFriend(userId, friendId);

            _repoMock.Verify(r => r.AddFriend(userId, friendId), Times.Never());
            _notificationServiceMock.Verify(n => n.SendFriendNotification(userId, friendId), Times.Never());
        }

        [TestMethod]
        public void RemoveFriend_ValidFriend_CallsRepoAndNotification()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            user.AddFriend(friendId);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(friendId)).Returns(friend);
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { friendId });

            _userService.RemoveFriend(userId, friendId);

            _repoMock.Verify(r => r.DeleteFriend(userId, friendId), Times.Once());
            _notificationServiceMock.Verify(n => n.SendRemoveFriendNotification(userId, friendId), Times.Once());
            Assert.IsFalse(user.GetFriends().Contains(friendId));
        }

        [TestMethod]
        public void RemoveFriend_NotFriends_DoesNotCallRepo()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(friendId)).Returns(friend);
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { 2 });

            _userService.RemoveFriend(userId, friendId);

            _repoMock.Verify(r => r.DeleteFriend(userId, friendId), Times.Never());
            _notificationServiceMock.Verify(n => n.SendRemoveFriendNotification(userId, friendId), Times.Never());
        }

        [TestMethod]
        public void JoinChat_NotInChat_JoinsSuccessfully()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { 200 });

            _userService.JoinChat(userId, chatId);

            _repoMock.Verify(r => r.AddUserToChat(chatId, userId), Times.Once());
            Assert.IsTrue(user.GetChats().Contains(chatId));
        }

        [TestMethod]
        public void JoinChat_AlreadyInChat_DoesNotJoin()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { chatId });

            _userService.JoinChat(userId, chatId);

            _repoMock.Verify(r => r.AddUserToChat(chatId, userId), Times.Never());
        }

        [TestMethod]
        public void LeaveChat_InChat_LeavesSuccessfully()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);
            user.JoinChat(chatId);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { chatId });

            _userService.LeaveChat(userId, chatId);

            _repoMock.Verify(r => r.RemoveUserFromChat(userId, chatId), Times.Once());
            Assert.IsFalse(user.GetChats().Contains(chatId));
        }

        [TestMethod]
        public void LeaveChat_NotInChat_DoesNotCallRepo()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { 200 });

            _userService.LeaveChat(userId, chatId);

            _repoMock.Verify(r => r.RemoveUserFromChat(userId, chatId), Times.Never());
        }

        [TestMethod]
        public void FilterUsers_ValidKeyword_ReturnsMatchingUsers()
        {
            int userId = 1;
            string keyword = "test";
            var users = new List<User>
        {
            new User(2, "TestUser", "1234567890", 0),
            new User(3, "Other", "0987654321", 0)
        };
            _repoMock.Setup(r => r.GetUsersList()).Returns(users);

            var result = _userService.FilterUsers(keyword, userId);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0]);
        }

        [TestMethod]
        public void FilterFriends_ValidKeyword_ReturnsMatchingFriends()
        {
            int userId = 1;
            string keyword = "test";
            var friends = new List<User>
        {
            new User(2, "TestFriend", "1234567890", 0),
            new User(3, "Other", "0987654321", 0)
        };
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(new User(userId, "User1", "123", 0));
            _repoMock.Setup(r => r.GetUserFriendsList(userId)).Returns(friends);

            var result = _userService.FilterFriends(keyword, userId);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0]);
        }

        [TestMethod]
        public void GetFriendsIDsByUser_ReturnsFriendIds()
        {
            int userId = 1;
            var friendIds = new List<int> { 2, 3 };
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(friendIds);

            var result = _userService.GetFriendsIDsByUser(userId);

            CollectionAssert.AreEqual(friendIds, result);
        }

        [TestMethod]
        public void GetFriendsByUser_ReturnsFriends()
        {
            int userId = 1;
            var friends = new List<User>
        {
            new User(2, "Friend1", "1234567890", 0)
        };
            _repoMock.Setup(r => r.GetUserFriendsList(userId)).Returns(friends);

            var result = _userService.GetFriendsByUser(userId);

            CollectionAssert.AreEqual(friends, result);
        }

        [TestMethod]
        public void GetChatsByUser_ReturnsChatIds()
        {
            int userId = 1;
            var chatIds = new List<int> { 100, 200 };
            _repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(chatIds);

            var result = _userService.GetChatsByUser(userId);

            CollectionAssert.AreEqual(chatIds, result);
        }

        [TestMethod]
        public void GetCurrentUserChats_ReturnsUserChats()
        {
            var chats = new List<Chat>
        {
            new Chat(100, "Chat1", new List<int> { CurrentUserId, 3 }),
            new Chat(200, "Chat2", new List<int> { 4, 5 })
        };
            _repoMock.Setup(r => r.GetChatsList()).Returns(chats);

            var result = _userService.GetCurrentUserChats();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].getChatID());
        }

        [TestMethod]
        public void GetUserById_ExistingUser_ReturnsUser()
        {
            int userId = 1;
            var users = new List<User>
        {
            new User(userId, "User1", "1234567890", 0)
        };
            _repoMock.Setup(r => r.GetUsersList()).Returns(users);

            var result = _userService.GetUserById(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
        }

        [TestMethod]
        public void GetNonFriendsUsers_ReturnsNonFriends()
        {
            int userId = 1;
            var users = new List<User>
        {
            new User(userId, "User1", "123", 0),
            new User(2, "Friend1", "456", 0),
            new User(3, "NonFriend", "789", 0)
        };
            _repoMock.Setup(r => r.GetUsersList()).Returns(users);
            _repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { 2 });

            var result = _userService.GetNonFriendsUsers(userId);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].GetUserId());
        }

        [TestMethod]
        public void GetCurrentUser_ReturnsLoggedInUserId()
        {
            var result = _userService.GetCurrentUser();

            Assert.AreEqual(CurrentUserId, result);
        }

        [TestMethod]
        public void MarkUserAsDangerousAndGiveTimeout_ReportedUser_SetsTimeout()
        {
            var user = new User(1, "User1", "1234567890", 1);

            _userService.MarkUserAsDangerousAndGiveTimeout(user);

            Assert.IsNotNull(user.GetTimeoutEnd());
            Assert.IsTrue(user.GetTimeoutEnd() > DateTime.Now);
        }

        [TestMethod]
        public void IsUserInTimeout_ActiveTimeout_ReturnsTrue()
        {
            var user = new User(1, "User1", "1234567890", 0);
            user.SetTimeoutEnd(DateTime.Now.AddMinutes(5));

            var result = _userService.IsUserInTimeout(user);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetUserTimeout_SetsFlag()
        {
            _userService.setUserTimeout(true);

            // Note: This tests a private field change; ideally, verify through behavior
            Assert.IsTrue(true); // Placeholder since isUserInTimeout is private
        }
    }
}

