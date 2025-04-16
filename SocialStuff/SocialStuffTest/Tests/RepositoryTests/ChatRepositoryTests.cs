using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Data.Database;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SocialStuffTest.Tests.RepositoryTests
{
    [TestClass]
    public class ChatRepositoryTests
    {
        private Mock<DatabaseConnection>? _mockDbConnection;
        private Repository? _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDbConnection = new Mock<DatabaseConnection>();
            _repository = new Repository(_mockDbConnection.Object);
        }

        [TestMethod]
        public void GetLoggedInUserID_WhenCalled_ReturnsConstantUserID()
        {
            // Arrange
            int expectedUserId = 2; // LOGGEDINUSERID constant

            // Act
            int actualUserId = _repository!.GetLoggedInUserID();

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        public void GetChatParticipantsIDs_WithValidChatID_ReturnsParticipantIDs()
        {
            // Arrange
            int chatId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("chatid", typeof(int));
            dataTable.Columns.Add("userid", typeof(int));
            dataTable.Rows.Add(chatId, 1);
            dataTable.Rows.Add(chatId, 2);
            _mockDbConnection!.Setup(db => db.ExecuteReader("select * from Chat_Participants", It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);
            var expectedIds = new List<int> { 1, 2 };

            // Act
            var actualIds = _repository!.GetChatParticipantsIDs(chatId);

            // Assert
            CollectionAssert.AreEqual(expectedIds, actualIds);
            _mockDbConnection.Verify(db => db.ExecuteReader("select * from Chat_Participants", It.IsAny<SqlParameter[]>(), false), Times.Once);
        }

        [TestMethod]
        public void GetChatParticipantsIDs_WithNoParticipants_ReturnsEmptyList()
        {
            // Arrange
            int chatId = 1;
            var dataTable = new DataTable();
            _mockDbConnection!.Setup(db => db.ExecuteReader("select * from Chat_Participants", It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            // Act
            var actualIds = _repository!.GetChatParticipantsIDs(chatId);

            // Assert
            CollectionAssert.AreEqual(new List<int>(), actualIds);
        }

        [TestMethod]
        public void AddRequestMessage_WithValidParameters_ExecutesNonQuery()
        {
            // Arrange
            int userId = 1;
            int chatId = 1;
            string content = "Request money";
            string status = "Pending";
            float amount = 50.0f;
            string currency = "USD";

            // Act
            _repository!.AddRequestMessage(userId, chatId, content, status, amount, currency);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "AddMessage",
                It.Is<SqlParameter[]>(p =>
                    (int)p[0].Value == 3 &&
                    (int)p[1].Value == userId &&
                    (int)p[2].Value == chatId &&
                    (string)p[3].Value == content &&
                    (string)p[4].Value == status &&
                    (float)p[5].Value == amount &&
                    (string)p[6].Value == currency
                )), Times.Once);
        }

        [TestMethod]
        public void AddRequestMessage_WithNullParameters_HandlesNullsCorrectly()
        {
            // Arrange
            int userId = 1;
            int chatId = 1;
            string content = "Request money";

            // Act
            _repository!.AddRequestMessage(userId, chatId, content);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "AddMessage",
                It.Is<SqlParameter[]>(p =>
                    (int)p[0].Value == 3 &&
                    (int)p[1].Value == userId &&
                    (int)p[2].Value == chatId &&
                    (string)p[3].Value == content &&
                    p[4].Value == DBNull.Value &&
                    p[5].Value == DBNull.Value &&
                    p[6].Value == DBNull.Value
                )), Times.Once);
        }

        [TestMethod]
        public void AddTransferMessage_WithValidParameters_ExecutesNonQuery()
        {
            // Arrange
            int userId = 1;
            int chatId = 1;
            string content = "Transfer money";
            string status = "Accepted";
            float amount = 100.0f;
            string currency = "USD";

            // Act
            _repository!.AddTransferMessage(userId, chatId, content, status, amount, currency);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "AddMessage",
                It.Is<SqlParameter[]>(p =>
                    (int)p[0].Value == 4 &&
                    (int)p[1].Value == userId &&
                    (int)p[2].Value == chatId &&
                    (string)p[3].Value == content &&
                    (string)p[4].Value == status &&
                    (float)p[5].Value == amount &&
                    (string)p[6].Value == currency
                )), Times.Once);
        }

        [TestMethod]
        public void AddChat_WithValidName_ReturnsNewChatID()
        {
            // Arrange
            string chatName = "Test Chat";
            int expectedChatId = 5;
            _mockDbConnection!.Setup(db => db.ExecuteNonQuery(
                "AddChat",
                It.Is<SqlParameter[]>(p =>
                    p[0].ParameterName == "@ChatName" &&
                    (string)p[0].Value == chatName &&
                    p[1].ParameterName == "@ChatID" &&
                    p[1].Direction == ParameterDirection.Output
                ))).Callback<string, SqlParameter[]>((name, p) => p[1].Value = expectedChatId);

            // Act
            int actualChatId = _repository!.AddChat(chatName);

            // Assert
            Assert.AreEqual(expectedChatId, actualChatId);
            _mockDbConnection.Verify(db => db.ExecuteNonQuery("AddChat", It.IsAny<SqlParameter[]>()), Times.Once);
        }

        [TestMethod]
        public void AddUserToChat_WithValidParameters_ExecutesNonQuery()
        {
            // Arrange
            int userId = 1;
            int chatId = 1;

            // Act
            _repository!.AddUserToChat(userId, chatId);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "AddUserToChat",
                It.Is<SqlParameter[]>(p =>
                    (int)p[0].Value == userId &&
                    (int)p[1].Value == chatId
                )), Times.Once);
        }

        [TestMethod]
        public void GetChatParticipants_WithValidChatID_ReturnsUsers()
        {
            // Arrange
            int chatId = 1;
            var participantsTable = new DataTable();
            participantsTable.Columns.Add("UserID", typeof(int));
            participantsTable.Rows.Add(1);
            participantsTable.Rows.Add(2);
            _mockDbConnection!.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chat_Participants WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => (int)p[0].Value == chatId),
                false
            )).Returns(participantsTable);

            var user1 = new User(1, "User1", "123", 0);
            var user2 = new User(2, "User2", "456", 0);
            _mockDbConnection.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => (int)p[0].Value == 1),
                false
            )).Returns(CreateUserDataTable(1, "User1", "123", 0));
            _mockDbConnection.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => (int)p[0].Value == 2),
                false
            )).Returns(CreateUserDataTable(2, "User2", "456", 0));

            // Act
            var participants = _repository!.GetChatParticipants(chatId);

            // Assert
            Assert.AreEqual(2, participants.Count);
            Assert.AreEqual("User1", participants[0].GetUsername());
            Assert.AreEqual("User2", participants[1].GetUsername());
            _mockDbConnection.Verify(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false), Times.Exactly(3));
        }

        [TestMethod]
        public void DeleteChat_WithValidChatID_ExecutesNonQuery()
        {
            // Arrange
            int chatId = 1;

            // Act
            _repository!.DeleteChat(chatId);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "DeleteChat",
                It.Is<SqlParameter[]>(p => (int)p[0].Value == chatId)
            ), Times.Once);
        }

        [TestMethod]
        public void GetMessagesList_WithMessages_ReturnsMessageList()
        {
            // Arrange
            var messagesTable = new DataTable();
            messagesTable.Columns.Add("messageid", typeof(int));
            messagesTable.Columns.Add("typeid", typeof(int));
            messagesTable.Columns.Add("userid", typeof(int));
            messagesTable.Columns.Add("chatid", typeof(int));
            messagesTable.Columns.Add("timestamp", typeof(DateTime));
            messagesTable.Columns.Add("content", typeof(string));
            messagesTable.Columns.Add("status", typeof(string));
            messagesTable.Columns.Add("amount", typeof(float));
            messagesTable.Columns.Add("currency", typeof(string));
            messagesTable.Rows.Add(1, 1, 1, 1, DateTime.Now, "Hello", null, null, null);

            var reportsTable = new DataTable();
            reportsTable.Columns.Add("messageid", typeof(int));
            _mockDbConnection!.Setup(db => db.ExecuteReader("select * from Messages", It.IsAny<SqlParameter[]>(), false))
                .Returns(messagesTable);
            _mockDbConnection.Setup(db => db.ExecuteReader("select * from Reports", It.IsAny<SqlParameter[]>(), false))
                .Returns(reportsTable);

            // Act
            var messages = _repository!.GetMessagesList();

            // Assert
            Assert.AreEqual(1, messages.Count);
            Assert.IsInstanceOfType(messages[0], typeof(TextMessage));
            Assert.AreEqual("Hello", ((TextMessage)messages[0]).GetContent());
            _mockDbConnection.Verify(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false), Times.Exactly(2));
        }

        [TestMethod]
        public void GetChatsList_WithChats_ReturnsChatList()
        {
            // Arrange
            var chatsTable = new DataTable();
            chatsTable.Columns.Add("chatid", typeof(int));
            chatsTable.Columns.Add("chatname", typeof(string));
            chatsTable.Rows.Add(1, "Test Chat");

            var participantsTable = new DataTable();
            participantsTable.Columns.Add("chatid", typeof(int));
            participantsTable.Columns.Add("userid", typeof(int));
            participantsTable.Rows.Add(1, 1);
            participantsTable.Rows.Add(1, 2);

            _mockDbConnection!.Setup(db => db.ExecuteReader("select * from Chats", It.IsAny<SqlParameter[]>(), false))
                .Returns(chatsTable);
            _mockDbConnection.Setup(db => db.ExecuteReader("select * from Chat_Participants", It.IsAny<SqlParameter[]>(), false))
                .Returns(participantsTable);

            // Act
            var chats = _repository!.GetChatsList();

            // Assert
            Assert.AreEqual(1, chats.Count);
            Assert.AreEqual("Test Chat", chats[0].getChatName());
            CollectionAssert.AreEqual(new List<int> { 1, 2 }, chats[0].getUserIDsList());
            _mockDbConnection.Verify(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false), Times.Exactly(2));
        }

        [TestMethod]
        public void RemoveUserFromChat_WithValidParameters_ExecutesNonQuery()
        {
            // Arrange
            int userId = 1;
            int chatId = 1;

            // Act
            _repository!.RemoveUserFromChat(userId, chatId);

            // Assert
            _mockDbConnection!.Verify(db => db.ExecuteNonQuery(
                "RemoveUserFromChat",
                It.Is<SqlParameter[]>(p =>
                    (int)p[0].Value == userId &&
                    (int)p[1].Value == chatId
                )), Times.Once);
        }

        private DataTable CreateUserDataTable(int userId, string username, string phoneNumber, int reportedCount)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("PhoneNumber", typeof(string));
            dataTable.Columns.Add("ReportedCount", typeof(int));
            dataTable.Rows.Add(userId, username, phoneNumber, reportedCount);
            return dataTable;
        }
    }
}

