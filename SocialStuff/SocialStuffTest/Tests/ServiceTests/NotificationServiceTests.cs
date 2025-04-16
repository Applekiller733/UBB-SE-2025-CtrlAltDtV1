using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using System.Collections.Generic;
using System;

namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class NotificationServiceTests
    {
        private Mock<IRepository>? _repoMock;
        private NotificationService? _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository>();
            _service = new NotificationService(_repoMock.Object);
        }

        [TestMethod]
        public void GetNotifications_Called_ReturnsNotifications()
        {
            // Arrange
            int userId = 1;
            var expectedNotifications = new List<Notification>
            {
                new Notification(1, DateTime.Now, "Notification 1", userId),
                new Notification(2, DateTime.Now, "Notification 2", userId)
            };
            _repoMock!.Setup(r => r.GetNotifications(userId)).Returns(expectedNotifications);

            // Act
            var result = _service!.GetNotifications(userId);

            // Assert
            Assert.AreEqual(expectedNotifications, result);
            _repoMock.Verify(r => r.GetNotifications(userId), Times.Once());
        }

        [TestMethod]
        public void SendFriendNotification_UsersExist_CallsAddNotification()
        {
            // Arrange
            int userId = 1;
            int newFriendId = 2;
            var user = new User(userId, "User1", "1234567890", 0);
            var newFriend = new User(newFriendId, "Friend1", "0987654321", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(newFriendId)).Returns(newFriend);

            // Act
            _service.SendFriendNotification(userId, newFriendId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(
                $"You added user {newFriend.GetUsername()} ({newFriend.GetPhoneNumber()}) to your friend list.", userId),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"User {user.GetUsername()} ({user.GetPhoneNumber()}) added you as a friend.", newFriendId),
                Times.Once());
        }

        [TestMethod]
        public void SendFriendNotification_UserNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int userId = 1;
            int newFriendId = 2;
            _repoMock!.Setup(r => r.GetUserById(userId)).Returns((User)null);
            _repoMock.Setup(r => r.GetUserById(newFriendId)).Returns(new User(newFriendId, "Friend1", "0987654321", 0));

            // Act
            _service.SendFriendNotification(userId, newFriendId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendFriendNotification_NewFriendNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int userId = 1;
            int newFriendId = 2;
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(new User(userId, "User1", "1234567890", 0));
            _repoMock.Setup(r => r.GetUserById(newFriendId)).Returns((User?)null);

            // Act
            _service.SendFriendNotification(userId, newFriendId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendRemoveFriendNotification_UsersExist_CallsAddNotification()
        {
            // Arrange
            int userId = 1;
            int oldFriendId = 2;
            var user = new User(userId, "User1", "1234567890", 0);
            var oldFriend = new User(oldFriendId, "Friend1", "0987654321", 0);
            _repoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _repoMock.Setup(r => r.GetUserById(oldFriendId)).Returns(oldFriend);

            // Act
            _service.SendRemoveFriendNotification(userId, oldFriendId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(
                $"You removed user {oldFriend.GetUsername()} ({oldFriend.GetPhoneNumber()}) from your friend list.", userId),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"User {user.GetUsername()} ({user.GetPhoneNumber()}) deleted you from their friend list. So selfish!", oldFriendId),
                Times.Once());
        }

        [TestMethod]
        public void SendRemoveFriendNotification_UserNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int userId = 1;
            int oldFriendId = 2;
            _repoMock!.Setup(r => r.GetUserById(userId)).Returns((User?)null);
            _repoMock.Setup(r => r.GetUserById(oldFriendId)).Returns(new User(oldFriendId, "Friend1", "0987654321", 0));

            // Act
            _service!.SendRemoveFriendNotification(userId, oldFriendId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendMessageNotification_ChatAndSenderExist_CallsAddNotification()
        {
            // Arrange
            int senderId = 1;
            int chatId = 1;
            var sender = new User(senderId, "Sender", "1234567890", 0);
            var chat = new Chat(chatId, "TestChat", new List<int> { 1, 2, 3 });
            var participants = new List<User>
            {
                new User(1, "Sender", "1234567890", 0),
                new User(2, "User2", "0987654321", 0),
                new User(3, "User3", "1122334455", 0)
            };
            _repoMock!.Setup(r => r.GetUserById(senderId)).Returns(sender);
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns(chat);
            _repoMock.Setup(r => r.GetChatParticipants(chatId)).Returns(participants);

            // Act
            _service!.SendMessageNotification(senderId, chatId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(
                $"User {sender.GetUsername()} sent a message in chat {chat.getChatName()}.", 2),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"User {sender.GetUsername()} sent a message in chat {chat.getChatName()}.", 3),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), senderId), Times.Never());
        }

        [TestMethod]
        public void SendMessageNotification_SenderNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int senderId = 1;
            int chatId = 1;
            _repoMock!.Setup(r => r.GetUserById(senderId)).Returns((User?)null);
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns(new Chat(chatId, "TestChat", new List<int> { 1, 2 }));

            // Act
            _service!.SendMessageNotification(senderId, chatId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendMessageNotification_ChatNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int senderId = 1;
            int chatId = 1;
            _repoMock!.Setup(r => r.GetUserById(senderId)).Returns(new User(senderId, "Sender", "1234567890", 0));
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns((Chat?)null);

            // Act
            _service!.SendMessageNotification(senderId, chatId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendTransactionNotification_ChatAndReceiverExist_CallsAddNotification()
        {
            // Arrange
            int receiverId = 1;
            int chatId = 1;
            string type = "Request";
            float amount = 100.50f;
            string currency = "USD";
            var receiver = new User(receiverId, "Receiver", "1234567890", 0);
            var chat = new Chat(chatId, "TestChat", new List<int> { 1, 2, 3 });
            var participants = new List<User>
            {
                new User(1, "Receiver", "1234567890", 0),
                new User(2, "User2", "0987654321", 0),
                new User(3, "User3", "1122334455", 0)
            };
            _repoMock!.Setup(r => r.GetUserById(receiverId)).Returns(receiver);
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns(chat);
            _repoMock.Setup(r => r.GetChatParticipants(chatId)).Returns(participants);

            // Act
            _service!.SendTransactionNotification(receiverId, chatId, type, amount, currency);

            // Assert
            _repoMock.Verify(r => r.AddNotification(
                $"You requested {amount} {currency} in {chat.getChatName()} group.", receiverId),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"User {receiver.GetUsername()} requested {amount} {currency} in {chat.getChatName()} group.", 2),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"User {receiver.GetUsername()} requested {amount} {currency} in {chat.getChatName()} group.", 3),
                Times.Once());
        }

        [TestMethod]
        public void SendTransactionNotification_ReceiverNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int receiverId = 1;
            int chatId = 1;
            string type = "Request";
            float amount = 100.50f;
            string currency = "USD";
            _repoMock!.Setup(r => r.GetUserById(receiverId)).Returns((User?)null);
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns(new Chat(chatId, "TestChat", new List<int> { 1, 2 }));

            // Act
            _service!.SendTransactionNotification(receiverId, chatId, type, amount, currency);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendTransactionNotification_ChatNotFound_DoesNotCallAddNotification()
        {
            // Arrange
            int receiverId = 1;
            int chatId = 1;
            string type = "Request";
            float amount = 100.50f;
            string currency = "USD";
            _repoMock!.Setup(r => r.GetUserById(receiverId)).Returns(new User(receiverId, "Receiver", "1234567890", 0));
            _repoMock.Setup(r => r.GetChatById(chatId)).Returns((Chat?)null);

            // Act
            _service!.SendTransactionNotification(receiverId, chatId, type, amount, currency);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void SendNewChatNotification_ParticipantsExist_CallsAddNotification()
        {
            // Arrange
            int chatId = 1;
            var participants = new List<User>
            {
                new User(1, "User1", "1234567890", 0),
                new User(2, "User2", "0987654321", 0)
            };
            _repoMock!.Setup(r => r.GetChatParticipants(chatId)).Returns(participants);

            // Act
            _service!.SendNewChatNotification(chatId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(
                $"You have been added to a new chat {chatId}.", 1),
                Times.Once());
            _repoMock.Verify(r => r.AddNotification(
                $"You have been added to a new chat {chatId}.", 2),
                Times.Once());
        }

        [TestMethod]
        public void SendNewChatNotification_NoParticipants_DoesNotCallAddNotification()
        {
            // Arrange
            int chatId = 1;
            _repoMock!.Setup(r => r.GetChatParticipants(chatId)).Returns(new List<User>());

            // Act
            _service!.SendNewChatNotification(chatId);

            // Assert
            _repoMock.Verify(r => r.AddNotification(It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void ClearNotification_Called_CallsDeleteNotification()
        {
            // Arrange
            int notificationId = 1;

            // Act
            _service!.ClearNotification(notificationId);

            // Assert
            _repoMock!.Verify(r => r.DeleteNotification(notificationId), Times.Once());
        }

        [TestMethod]
        public void ClearAllNotifications_Called_CallsClearAllNotifications()
        {
            // Arrange
            int userId = 1;

            // Act
            _service!.ClearAllNotifications(userId);

            // Assert
            _repoMock!.Verify(r => r.ClearAllNotifications(userId), Times.Once());
        }
    }
}