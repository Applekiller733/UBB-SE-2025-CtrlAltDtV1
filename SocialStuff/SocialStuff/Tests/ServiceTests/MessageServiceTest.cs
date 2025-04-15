using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace SocialStuff.Tests.ServiceTests
{
    [TestClass]
    public class MessageServiceTests
    {
        private Mock<IRepository> _repoMock;
        private Mock<IUserService> _userServiceMock;
        private MessageService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository>();
            _userServiceMock = new Mock<IUserService>();
            _service = new MessageService(_repoMock.Object, _userServiceMock.Object);
        }

        [TestMethod]
        public void GetRepo_Called_ReturnsRepository()
        {
            // Arrange
            var expectedRepo = _repoMock.Object;

            // Act
            var result = _service.getRepo();

            // Assert
            Assert.AreEqual(expectedRepo, result);
        }

        [TestMethod]
        public void SendMessage_UserNotInTimeout_CallsAddTextMessage()
        {
            // Arrange
            int senderId = 1, chatId = 2;
            string content = "Hello";
            _userServiceMock.Setup(u => u.GetUserById(senderId)).Returns(new User(senderId, "user", "123", 0));
            _userServiceMock.Setup(u => u.IsUserInTimeout(It.IsAny<User>())).Returns(false);

            // Act
            _service.sendMessage(senderId, chatId, content);

            // Assert
            _repoMock.Verify(r => r.AddTextMessage(senderId, chatId, content), Times.Once());
        }

        [TestMethod]
        public void SendMessage_UserInTimeout_DoesNotCallAddTextMessage()
        {
            // Arrange
            int senderId = 1, chatId = 2;
            string content = "Hello";
            _userServiceMock.Setup(u => u.GetUserById(senderId)).Returns(new User(senderId, "user", "123", 0));
            _userServiceMock.Setup(u => u.IsUserInTimeout(It.IsAny<User>())).Returns(true);

            // Act
            _service.sendMessage(senderId, chatId, content);

            // Assert
            _repoMock.Verify(r => r.AddTextMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void SendMessage_UserNotFound_DoesNotCallAddTextMessage()
        {
            // Arrange
            int senderId = 1, chatId = 2;
            string content = "Hello";
            _userServiceMock.Setup(u => u.GetUserById(senderId)).Returns((User)null);

            // Act
            _service.sendMessage(senderId, chatId, content);

            // Assert
            _repoMock.Verify(r => r.AddTextMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void SendImage_UserNotInTimeout_CallsAddImageMessage()
        {
            // Arrange
            int senderId = 1, chatId = 2;
            string imageUrl = "http://image.com";
            _userServiceMock.Setup(u => u.GetUserById(senderId)).Returns(new User(senderId, "user", "123", 0));
            _userServiceMock.Setup(u => u.IsUserInTimeout(It.IsAny<User>())).Returns(false);

            // Act
            _service.sendImage(senderId, chatId, imageUrl);

            // Assert
            _repoMock.Verify(r => r.AddImageMessage(senderId, chatId, imageUrl), Times.Once());
        }

        [TestMethod]
        public void SendImage_UserInTimeout_DoesNotCallAddImageMessage()
        {
            // Arrange
            int senderId = 1, chatId = 2;
            string imageUrl = "http://image.com";
            _userServiceMock.Setup(u => u.GetUserById(senderId)).Returns(new User(senderId, "user", "123", 0));
            _userServiceMock.Setup(u => u.IsUserInTimeout(It.IsAny<User>())).Returns(true);

            // Act
            _service.sendImage(senderId, chatId, imageUrl);

            // Assert
            _repoMock.Verify(r => r.AddImageMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void DeleteMessage_Called_CallsDeleteMessage()
        {
            // Arrange
            var message = new TextMessage(1, 1, 2, DateTime.Now, "content", new List<int>());
            _repoMock.Setup(r => r.DeleteMessage(message.getMessageID())).Verifiable();

            // Act
            _service.deleteMessage(message);

            // Assert
            _repoMock.Verify(r => r.DeleteMessage(message.getMessageID()), Times.Once());
        }

        [TestMethod]
        public void SendTransferMessage_Called_CallsAddTransferMessage()
        {
            // Arrange
            int userId = 1, chatId = 2;
            string content = "Transfer", status = "Pending", currency = "USD";
            float amount = 100.0f;

            // Act
            _service.sendTransferMessage(userId, chatId, content, status, amount, currency);

            // Assert
            _repoMock.Verify(r => r.AddTransferMessage(userId, chatId, content, status, amount, currency), Times.Once());
        }

        [TestMethod]
        public void SendRequestMessage_Called_CallsAddRequestMessage()
        {
            // Arrange
            int userId = 1, chatId = 2;
            string content = "Request", status = "Pending", currency = "USD";
            float amount = 50.0f;

            // Act
            _service.sendRequestMessage(userId, chatId, content, status, amount, currency);

            // Assert
            _repoMock.Verify(r => r.AddRequestMessage(userId, chatId, content, status, amount, currency), Times.Once());
        }

        [TestMethod]
        public void ReportMessage_Called_DoesNotThrow()
        {
            // Arrange
            var message = new TextMessage(1, 1, 2, DateTime.Now, "content", new List<int>());

            // Act
            _service.ReportMessage(message);

            // Assert
            Assert.IsTrue(true); // Since method is empty, ensure it doesn't throw
        }
    }
}