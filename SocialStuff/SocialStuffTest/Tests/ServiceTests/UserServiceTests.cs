// <copyright file="UserServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace SocialStuff.Tests.ServiceTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SocialStuff.Data;
    using SocialStuff.Model;
    using SocialStuff.Services.Implementations;
    using SocialStuff.Services.Interfaces;

    [TestClass]
    public class UserServiceTests
    {
        private const int CurrentUserId = 2;
        private Mock<IRepository>? repoMock;
        private Mock<INotificationService>? notificationServiceMock;
        private UserService? userService;

        [TestInitialize]
        public void Setup()
        {
            this.repoMock = new Mock<IRepository>();
            this.notificationServiceMock = new Mock<INotificationService>();
            this.repoMock.Setup(r => r.GetLoggedInUserID()).Returns(CurrentUserId);
            this.userService = new UserService(this.repoMock.Object, this.notificationServiceMock.Object);
        }

        [TestMethod]
        public void AddFriend_ValidUsersNotFriends_CallsRepoAndNotification()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);

            // Properly mock the repository methods
            this.repoMock!.Setup(r => r.GetUserById(userId)).Returns(user);
            this.repoMock.Setup(r => r.GetUserById(friendId)).Returns(friend);
            // Ensure that GetFriendsIDs is setup properly to return an empty list
            this.repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { });

            // Call the method under test
            this.userService!.AddFriend(userId, friendId);

            // Verify AddFriend was called on the repo
            this.repoMock.Verify(r => r.AddFriend(userId, friendId), Times.Once());
            // Verify SendFriendNotification was called
            this.notificationServiceMock!.Verify(n => n.SendFriendNotification(userId, friendId), Times.Once());
        }

        [TestMethod]
        public void AddFriend_AlreadyFriends_DoesNotCallRepo()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            this.repoMock!.Setup(r => r.GetUserById(userId)).Returns(user);
            this.repoMock!.Setup(r => r.GetUserById(friendId)).Returns(friend);
            this.repoMock!.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { friendId });

            this.userService!.AddFriend(userId, friendId);

            this.repoMock.Verify(r => r.AddFriend(userId, friendId), Times.Never());
            this.notificationServiceMock!.Verify(n => n.SendFriendNotification(userId, friendId), Times.Never());
        }

        [TestMethod]
        public void RemoveFriend_ValidFriend_CallsRepoAndNotification()
        {
            int userId = 1;
            int friendId = 3;
            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);
            user.AddFriend(friendId);

            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user, friend });
            this.repoMock!.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { friendId });

            this.userService!.RemoveFriend(userId, friendId);

            this.repoMock.Verify(r => r.DeleteFriend(userId, friendId), Times.Once());
            this.notificationServiceMock!.Verify(n => n.SendRemoveFriendNotification(userId, friendId), Times.Once());
            Assert.IsFalse(user.GetFriends().Contains(friendId));
        }

        [TestMethod]
        public void RemoveFriend_NotFriends_DoesNotCallRepo()
        {
            int userId = 1;
            int friendId = 3;

            var user = new User(userId, "User1", "1234567890", 0);
            var friend = new User(friendId, "Friend1", "0987654321", 0);

            // ✅ Fix: mock GetUsersList with both user and friend
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user, friend });

            // ✅ user is NOT friends with friendId
            this.repoMock!.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { 2 });

            this.userService!.RemoveFriend(userId, friendId);

            this.repoMock.Verify(r => r.DeleteFriend(userId, friendId), Times.Never());
            this.notificationServiceMock!.Verify(n => n.SendRemoveFriendNotification(userId, friendId), Times.Never());
        }


        [TestMethod]
        public void JoinChat_NotInChat_JoinsSuccessfully()
        {
            int userId = 1;
            int chatId = 100;

            var user = new User(userId, "User1", "1234567890", 0);

            // Fix: mock GetUsersList instead of GetUserById
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user });

            // Mock that the user is NOT in the chat yet
            this.repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { 200 });

            // Act
            this.userService!.JoinChat(userId, chatId);

            // Assert
            this.repoMock.Verify(r => r.AddUserToChat(chatId, userId), Times.Once());
            Assert.IsTrue(user.GetChats().Contains(chatId));
        }


        [TestMethod]
        public void JoinChat_AlreadyInChat_DoesNotJoin()
        {
            int userId = 1;
            int chatId = 100;

            var user = new User(userId, "User1", "1234567890", 0);

            // Mock GetUsersList() because GetUserById() in UserService loops through it
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user });

            // Simulate that user is already in the chat
            this.repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { chatId });

            // Act
            this.userService!.JoinChat(userId, chatId);

            // Assert: Ensure AddUserToChat was never called because user was already in chat
            this.repoMock.Verify(r => r.AddUserToChat(chatId, userId), Times.Never());
        }


        [TestMethod]
        public void LeaveChat_InChat_LeavesSuccessfully()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);
            user.JoinChat(chatId); // manually add chat to user

            // ✅ Mock GetUsersList() so GetUserById() can find the user
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user });

            // ✅ Mock chat membership
            this.repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { chatId });

            // Act
            this.userService!.LeaveChat(userId, chatId);

            // Assert
            this.repoMock.Verify(r => r.RemoveUserFromChat(userId, chatId), Times.Once());
            Assert.IsFalse(user.GetChats().Contains(chatId));
        }


        [TestMethod]
        public void LeaveChat_NotInChat_DoesNotCallRepo()
        {
            int userId = 1;
            int chatId = 100;
            var user = new User(userId, "User1", "1234567890", 0);

            // ✅ Fix: mock GetUsersList, not GetUserById
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(new List<User> { user });

            // ✅ User is NOT in chat 100
            this.repoMock.Setup(r => r.GetChatsIDs(userId)).Returns(new List<int> { 200 });

            this.userService!.LeaveChat(userId, chatId);

            this.repoMock.Verify(r => r.RemoveUserFromChat(userId, chatId), Times.Never());
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
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(users);

            var result = this.userService!.FilterUsers(keyword, userId);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0]);
        }

        [TestMethod]
        public void FilterFriends_WithMatchingKeyword_ReturnsExpectedFriendIds()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            var userId = 1;
            var keyword = "test";

            // Full list of users (used by GetUserById)
            var users = new List<User>
            {
                new User(1, "MainUser", "0000000000", 0),
                new User(2, "TestFriend", "1111111111", 0),     // match by username
                new User(3, "NoMatch", "2222222222", 0),
                new User(4, "Another", "4444test555", 0)        // match by phone number
            };

            // Friends list (used by GetUserFriendsList)
            var friends = new List<User>
            {
                users[1], // ID 2
                users[2], // ID 3
                users[3]  // ID 4
            };

            repoMock.Setup(r => r.GetUsersList()).Returns(users);
            repoMock.Setup(r => r.GetUserFriendsList(userId)).Returns(friends);

            var userService = new UserService(repoMock.Object, notificationServiceMock.Object);

            // Act
            var result = userService.FilterFriends(keyword, userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(new List<int> { 2, 4 }, result);
        }

        [TestMethod]
        public void GetFriendsIDsByUser_ReturnsFriendIds()
        {
            int userId = 1;
            var friendIds = new List<int> { 2, 3 };
            this.repoMock!.Setup(r => r.GetFriendsIDs(userId)).Returns(friendIds);

            var result = this.userService!.GetFriendsIDsByUser(userId);

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
            this.repoMock!.Setup(r => r.GetUserFriendsList(userId)).Returns(friends);

            var result = this.userService!.GetFriendsByUser(userId);

            CollectionAssert.AreEqual(friends, result);
        }

        [TestMethod]
        public void GetChatsByUser_ReturnsChatIds()
        {
            int userId = 1;
            var chatIds = new List<int> { 100, 200 };
            this.repoMock!.Setup(r => r.GetChatsIDs(userId)).Returns(chatIds);

            var result = this.userService!.GetChatsByUser(userId);

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
            this.repoMock!.Setup(r => r.GetChatsList()).Returns(chats);

            var result = this.userService!.GetCurrentUserChats();

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
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(users);

            var result = this.userService!.GetUserById(userId);

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
            this.repoMock!.Setup(r => r.GetUsersList()).Returns(users);
            this.repoMock.Setup(r => r.GetFriendsIDs(userId)).Returns(new List<int> { 2 });

            var result = this.userService!.GetNonFriendsUsers(userId);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].GetUserId());
        }

        [TestMethod]
        public void GetCurrentUser_ReturnsLoggedInUserId()
        {
            var result = this.userService!.GetCurrentUser();

            Assert.AreEqual(CurrentUserId, result);
        }

        [TestMethod]
        public void MarkUserAsDangerousAndGiveTimeout_ReportedUser_SetsTimeout()
        {
            var user = new User(1, "User1", "1234567890", 1);

            this.userService!.MarkUserAsDangerousAndGiveTimeout(user);

            Assert.IsNotNull(user.GetTimeoutEnd());
            Assert.IsTrue(user.GetTimeoutEnd() > DateTime.Now);
        }

        [TestMethod]
        public void IsUserInTimeout_ActiveTimeout_ReturnsTrue()
        {
            var user = new User(1, "User1", "1234567890", 0);
            user.SetTimeoutEnd(DateTime.Now.AddMinutes(5));

            var result = this.userService!.IsUserInTimeout(user);

            Assert.IsTrue(result);
        }
    }
}