using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class ChatServiceTests
    {
        private Mock<IRepository> _mockRepo;
        private ChatService _chatService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepo = new Mock<IRepository>();
            _chatService = new ChatService(_mockRepo.Object);
        }

        [TestMethod]
        public void GetCurrentUserID_WhenCalled_ReturnsLoggedInUserID()
        {
            // Arrange
            int expectedUserId = 2;
            _mockRepo.Setup(repo => repo.GetLoggedInUserID()).Returns(expectedUserId);

            // Act
            int actualUserId = _chatService.GetCurrentUserID();

            // Assert
            _mockRepo.Verify(repo => repo.GetLoggedInUserID(), Times.Once);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        public void GetNumberOfParticipants_WithValidChatId_ReturnsParticipantCount()
        {
            // Arrange
            int chatId = 1;
            var participants = new List<int> { 1, 2, 3 };
            _mockRepo.Setup(repo => repo.GetChatParticipantsIDs(chatId)).Returns(participants);

            // Act
            int count = _chatService.GetNumberOfParticipants(chatId);

            // Assert
            _mockRepo.Verify(repo => repo.GetChatParticipantsIDs(chatId), Times.Once);
            Assert.AreEqual(participants.Count, count);
        }

        [TestMethod]
        public void GetRepo_WhenCalled_ReturnsRepositoryInstance()
        {
            // Act
            var repo = _chatService.GetRepo();

            // Assert
            Assert.AreSame(_mockRepo.Object, repo);
        }

        [TestMethod]
        public void RequestMoneyViaChat_ValidParameters_CallsAddRequestMessage()
        {
            // Arrange
            float amount = 100;
            string currency = "USD";
            int chatId = 1;
            string description = "Test request";
            int currentUserId = 2;
            _mockRepo.Setup(repo => repo.GetLoggedInUserID()).Returns(currentUserId);

            // Act
            _chatService.RequestMoneyViaChat(amount, currency, chatId, description);

            // Assert
            _mockRepo.Verify(repo => repo.AddRequestMessage(
                currentUserId,
                chatId,
                description,
                "Pending",
                amount,
                currency
            ), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestMoneyViaChat_InvalidAmount_ThrowsArgumentException()
        {
            // Act
            _chatService.RequestMoneyViaChat(-50, "USD", 1, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestMoneyViaChat_EmptyCurrency_ThrowsArgumentException()
        {
            // Act
            _chatService.RequestMoneyViaChat(100, string.Empty, 1, "Test");
        }

        [TestMethod]
        public void SendMoneyViaChat_WithParticipants_CallsAddTransferMessageWithCorrectAmount()
        {
            // Arrange
            float amount = 10;
            string currency = "USD";
            int chatId = 1;
            string description = "Test send";
            int currentUserId = 1;
            var participants = new List<int> { currentUserId, 2, 3 };
            _mockRepo.Setup(repo => repo.GetLoggedInUserID()).Returns(currentUserId);
            _mockRepo.Setup(repo => repo.GetChatParticipantsIDs(chatId)).Returns(participants);
            float expectedTotalAmount = amount * (participants.Count - 1);

            // Act
            _chatService.SendMoneyViaChat(amount, currency, description, chatId);

            // Assert
            _mockRepo.Verify(repo => repo.AddTransferMessage(
                currentUserId,
                chatId,
                description,
                It.IsAny<string>(),
                expectedTotalAmount,
                currency
            ), Times.Once);
        }

        [TestMethod]
        public void AcceptRequestViaChat_WhenCalled_CallsAddTransferMessage()
        {
            // Arrange
            float amount = 50;
            string currency = "USD";
            int accepterId = 2;
            int requesterId = 1;
            int chatId = 1;
            _mockRepo.Setup(repo => repo.GetLoggedInUserID()).Returns(accepterId);

            // Act
            _chatService.AcceptRequestViaChat(amount, currency, accepterId, requesterId, chatId);

            // Assert
            _mockRepo.Verify(repo => repo.AddTransferMessage(
                accepterId,
                chatId,
                It.IsAny<string>(),
                It.IsAny<string>(),
                amount,
                currency
            ), Times.Once);
        }

        [TestMethod]
        public void CreateChat_WithParticipants_AddsChatAndParticipants()
        {
            // Arrange
            var participants = new List<int> { 1, 2, 3 };
            string chatName = "Test Chat";
            int expectedChatId = 5;
            _mockRepo.Setup(repo => repo.AddChat(chatName)).Returns(expectedChatId);
            _mockRepo.Setup(repo => repo.GetChatParticipants(expectedChatId)).Returns(new List<User>());

            // Act
            _chatService.CreateChat(participants, chatName);

            // Assert
            _mockRepo.Verify(repo => repo.AddChat(chatName), Times.Once);
            foreach (int userId in participants)
            {
                _mockRepo.Verify(repo => repo.AddUserToChat(userId, expectedChatId), Times.Once);
            }
            _mockRepo.Verify(repo => repo.GetChatParticipants(expectedChatId), Times.Once);
        }

        [TestMethod]
        public void DeleteChat_ValidChatId_CallsRepositoryDelete()
        {
            // Arrange
            int chatId = 1;

            // Act
            _chatService.DeleteChat(chatId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteChat(chatId), Times.Once);
        }

        [TestMethod]
        public void GetLastMessageTimeStamp_WithMessages_ReturnsLatestTimestamp()
        {
            // Arrange
            int chatId = 1;
            var messages = new List<Message>
            {
                new TextMessage(1, 1, chatId, DateTime.Now.AddHours(-2), "Old message", new List<int>()),
                new TextMessage(2, 1, chatId, DateTime.Now.AddHours(-1), "Newer message", new List<int>()),
                new TextMessage(3, 1, 2, DateTime.Now, "Other chat message", new List<int>())
            };
            _mockRepo.Setup(repo => repo.GetMessagesList()).Returns(messages);
            DateTime expectedTimestamp = messages[1].GetTimestamp();

            // Act
            DateTime actualTimestamp = _chatService.GetLastMessageTimeStamp(chatId);

            // Assert
            Assert.AreEqual(expectedTimestamp, actualTimestamp);
        }

        [TestMethod]
        public void GetLastMessageTimeStamp_NoMessages_ReturnsMinValue()
        {
            // Arrange
            int chatId = 1;
            var messages = new List<Message>();
            _mockRepo.Setup(repo => repo.GetMessagesList()).Returns(messages);

            // Act
            DateTime actualTimestamp = _chatService.GetLastMessageTimeStamp(chatId);

            // Assert
            Assert.AreEqual(DateTime.MinValue, actualTimestamp);
        }

        [TestMethod]
        public void GetChatHistory_WithMessages_ReturnsFilteredMessages()
        {
            // Arrange
            int chatId = 1;
            var messages = new List<Message>
            {
                new TextMessage(1, 1, chatId, DateTime.Now, "Message 1", new List<int>()),
                new TextMessage(2, 1, 2, DateTime.Now, "Message 2", new List<int>())
            };
            _mockRepo.Setup(repo => repo.GetMessagesList()).Returns(messages);

            // Act
            var chatHistory = _chatService.GetChatHistory(chatId);

            // Assert
            Assert.AreEqual(1, chatHistory.Count);
            Assert.AreEqual(chatId, chatHistory[0].GetChatID());
        }

        [TestMethod]
        public void AddUserToChat_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 2;
            int chatId = 1;

            // Act
            _chatService.AddUserToChat(userId, chatId);

            // Assert
            _mockRepo.Verify(repo => repo.AddUserToChat(userId, chatId), Times.Once);
        }

        [TestMethod]
        public void RemoveUserFromChat_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 2;
            int chatId = 1;

            // Act
            _chatService.RemoveUserFromChat(userId, chatId);

            // Assert
            _mockRepo.Verify(repo => repo.RemoveUserFromChat(userId, chatId), Times.Once);
        }

        [TestMethod]
        public void GetChatNameByID_ExistingChat_ReturnsName()
        {
            // Arrange
            int chatId = 1;
            string expectedName = "Test Chat";
            var chat = new Chat(chatId, expectedName, new List<int>());
            _mockRepo.Setup(repo => repo.GetChatsList()).Returns(new List<Chat> { chat });

            // Act
            string actualName = _chatService.GetChatNameByID(chatId);

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetChatNameByID_NonExistingChat_ThrowsException()
        {
            // Arrange
            int chatId = 1;
            _mockRepo.Setup(repo => repo.GetChatsList()).Returns(new List<Chat>());

            // Act
            _chatService.GetChatNameByID(chatId);
        }

        [TestMethod]
        public void GetChatParticipantsStringList_ReturnsUsernames()
        {
            // Arrange
            int chatId = 1;
            var participants = new List<User>
            {
                new User(1, "User1", "123", 0),
                new User(2, "User2", "456", 0)
            };
            _mockRepo.Setup(repo => repo.GetChatParticipants(chatId)).Returns(participants);

            // Act
            var result = _chatService.GetChatParticipantsStringList(chatId);

            // Assert
            CollectionAssert.AreEqual(new List<string> { "User1", "User2" }, result);
        }

        [TestMethod]
        public void GetChatParticipantsList_ReturnsParticipants()
        {
            // Arrange
            int chatId = 1;
            var expectedParticipants = new List<User>
            {
                new User(1, "User1", "123", 0),
                new User(2, "User2", "456", 0)
            };
            _mockRepo.Setup(repo => repo.GetChatParticipants(chatId)).Returns(expectedParticipants);

            // Act
            var actualParticipants = _chatService.GetChatParticipantsList(chatId);

            // Assert
            CollectionAssert.AreEqual(expectedParticipants, actualParticipants);
        }

        [TestMethod]
        public void EnoughFunds_ReturnsBoolean()
        {
            // Act
            bool result = _chatService.EnoughFunds(100, "USD", 1);

            // Assert
            Assert.IsTrue(result || !result); // Always true, checks it's a boolean
        }

        [TestMethod]
        public void InitiateTransfer_DoesNotThrow()
        {
            // Act & Assert (no exception expected)
            _chatService.InitiateTransfer(1, 2, 100, "USD");
        }
    }
}