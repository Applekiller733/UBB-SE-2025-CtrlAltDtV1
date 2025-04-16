using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Data.Database;
using SocialStuff.Model;
using System.Collections.Generic;
using System.Data;
using System;
using Microsoft.Data.SqlClient;

namespace SocialStuff.Tests.RepositoryTests
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private Mock<DatabaseConnection>? _dbConnectionMock;
        private Repository? _repository;

        [TestInitialize]
        public void Setup()
        {
            _dbConnectionMock = new Mock<DatabaseConnection>();
            _repository = new Repository();
            if (_repository != null)
            {
                // Inject mock DatabaseConnection using reflection
                typeof(Repository)
                    .GetField("dbConnection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(_repository, _dbConnectionMock.Object);
            }
        }

        [TestMethod]
        public void GetNotifications_DataExists_ReturnsNotifications()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("NotifID", typeof(int));
            dataTable.Columns.Add("Timestamp", typeof(DateTime));
            dataTable.Columns.Add("Content", typeof(string));
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Rows.Add(1, DateTime.Now, "Notification 1", userId);
            dataTable.Rows.Add(2, DateTime.Now, "Notification 2", userId);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Notifications WHERE UserID = @UserID ORDER BY Timestamp DESC",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == userId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetNotifications(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].NotificationID);
            Assert.AreEqual("Notification 1", result[0].Content);
            Assert.AreEqual(userId, result[0].UserReceiverID);
            Assert.AreEqual("Notification 2", result[1].Content);
            Assert.AreEqual(userId, result[1].UserReceiverID);
        }

        [TestMethod]
        public void GetNotifications_NoData_ReturnsEmptyList()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Notifications WHERE UserID = @UserID ORDER BY Timestamp DESC",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == userId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetNotifications(userId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void AddNotification_Called_CallsExecuteNonQuery()
        {
            // Arrange
            string content = "Test notification";
            int userId = 1;
            SqlParameter[]? capturedParameters = null;
            _dbConnectionMock!.Setup(db => db.ExecuteNonQuery(
                "AddNotification",
                It.IsAny<SqlParameter[]>()))
                .Callback<string, SqlParameter[]>((_, p) => capturedParameters = p);

            // Act
            _repository!.AddNotification(content, userId);

            // Assert
            _dbConnectionMock.Verify(db => db.ExecuteNonQuery(
                "AddNotification",
                It.IsAny<SqlParameter[]>()), Times.Once());
            Assert.AreEqual(2, capturedParameters!.Length);
            Assert.AreEqual("@Content", capturedParameters[0].ParameterName);
            Assert.AreEqual(content, capturedParameters[0].Value);
            Assert.AreEqual("@UserID", capturedParameters[1].ParameterName);
            Assert.AreEqual(userId, capturedParameters[1].Value);
        }

        [TestMethod]
        public void DeleteNotification_Called_CallsExecuteNonQuery()
        {
            // Arrange
            int notifId = 1;
            SqlParameter[] capturedParameters = null;
            _dbConnectionMock.Setup(db => db.ExecuteNonQuery(
                "DeleteNotification",
                It.IsAny<SqlParameter[]>()))
                .Callback<string, SqlParameter[]>((_, p) => capturedParameters = p);

            // Act
            _repository!.DeleteNotification(notifId);

            // Assert
            _dbConnectionMock.Verify(db => db.ExecuteNonQuery(
                "DeleteNotification",
                It.IsAny<SqlParameter[]>()), Times.Once());
            Assert.AreEqual(1, capturedParameters!.Length);
            Assert.AreEqual("@NotifID", capturedParameters[0].ParameterName);
            Assert.AreEqual(notifId, capturedParameters[0].Value);
        }

        [TestMethod]
        public void ClearAllNotifications_Called_CallsExecuteNonQuery()
        {
            // Arrange
            int userId = 1;
            SqlParameter[]? capturedParameters = null;
            _dbConnectionMock!.Setup(db => db.ExecuteNonQuery(
                "DeleteAllNotifications",
                It.IsAny<SqlParameter[]>()))
                .Callback<string, SqlParameter[]>((_, p) => capturedParameters = p);

            // Act
            _repository.ClearAllNotifications(userId);

            // Assert
            _dbConnectionMock.Verify(db => db.ExecuteNonQuery(
                "DeleteAllNotifications",
                It.IsAny<SqlParameter[]>()), Times.Once());
            Assert.AreEqual(1, capturedParameters!.Length);
            Assert.AreEqual("@UserID", capturedParameters[0].ParameterName);
            Assert.AreEqual(userId, capturedParameters[0].Value);
        }

        [TestMethod]
        public void GetUserById_DataExists_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("PhoneNumber", typeof(string));
            dataTable.Columns.Add("ReportedCount", typeof(int));
            dataTable.Rows.Add(userId, "TestUser", "1234567890", 0);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == userId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
            Assert.AreEqual("TestUser", result.GetUsername());
            Assert.AreEqual("1234567890", result.GetPhoneNumber());
            Assert.AreEqual(0, result.GetReportedCount());
        }

        [TestMethod]
        public void GetUserById_NoData_ReturnsNull()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == userId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetUserById(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetChatById_DataExists_ReturnsChat()
        {
            // Arrange
            int chatId = 1;
            var chatDataTable = new DataTable();
            chatDataTable.Columns.Add("ChatID", typeof(int));
            chatDataTable.Columns.Add("ChatName", typeof(string));
            chatDataTable.Rows.Add(chatId, "TestChat");

            var participantsDataTable = new DataTable();
            participantsDataTable.Columns.Add("UserID", typeof(int));
            participantsDataTable.Rows.Add(1);
            participantsDataTable.Rows.Add(2);

            var userDataTable1 = new DataTable();
            userDataTable1.Columns.Add("UserID", typeof(int));
            userDataTable1.Columns.Add("Username", typeof(string));
            userDataTable1.Columns.Add("PhoneNumber", typeof(string));
            userDataTable1.Columns.Add("ReportedCount", typeof(int));
            userDataTable1.Rows.Add(1, "User1", "1234567890", 0);

            var userDataTable2 = new DataTable();
            userDataTable2.Columns.Add("UserID", typeof(int));
            userDataTable2.Columns.Add("Username", typeof(string));
            userDataTable2.Columns.Add("PhoneNumber", typeof(string));
            userDataTable2.Columns.Add("ReportedCount", typeof(int));
            userDataTable2.Rows.Add(2, "User2", "0987654321", 0);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chats WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@ChatID" && (int)p[0].Value == chatId),
                false))
                .Returns(chatDataTable);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chat_Participants WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@ChatID" && (int)p[0].Value == chatId),
                false))
                .Returns(participantsDataTable);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == 1),
                false))
                .Returns(userDataTable1);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == 2),
                false))
                .Returns(userDataTable2);

            // Act
            var result = _repository.GetChatById(chatId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(chatId, result.getChatID());
            Assert.AreEqual("TestChat", result.getChatName());
            CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, result.getUserIDsList());
        }

        [TestMethod]
        public void GetChatById_NoData_ReturnsNull()
        {
            // Arrange
            int chatId = 1;
            var dataTable = new DataTable();
            _dbConnectionMock!.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chats WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@ChatID" && (int)p[0].Value == chatId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository!.GetChatById(chatId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetChatParticipants_DataExists_ReturnsParticipants()
        {
            // Arrange
            int chatId = 1;
            var participantsDataTable = new DataTable();
            participantsDataTable.Columns.Add("UserID", typeof(int));
            participantsDataTable.Rows.Add(1);
            participantsDataTable.Rows.Add(2);

            var userDataTable1 = new DataTable();
            userDataTable1.Columns.Add("UserID", typeof(int));
            userDataTable1.Columns.Add("Username", typeof(string));
            userDataTable1.Columns.Add("PhoneNumber", typeof(string));
            userDataTable1.Columns.Add("ReportedCount", typeof(int));
            userDataTable1.Rows.Add(1, "User1", "1234567890", 0);

            var userDataTable2 = new DataTable();
            userDataTable2.Columns.Add("UserID", typeof(int));
            userDataTable2.Columns.Add("Username", typeof(string));
            userDataTable2.Columns.Add("PhoneNumber", typeof(string));
            userDataTable2.Columns.Add("ReportedCount", typeof(int));
            userDataTable2.Rows.Add(2, "User2", "0987654321", 0);

            _dbConnectionMock!.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chat_Participants WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@ChatID" && (int)p[0].Value == chatId),
                false))
                .Returns(participantsDataTable);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == 1),
                false))
                .Returns(userDataTable1);

            _dbConnectionMock.Setup(db => db.ExecuteReader(
                "SELECT * FROM Users WHERE UserID = @UserID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@UserID" && (int)p[0].Value == 2),
                false))
                .Returns(userDataTable2);

            // Act
            var result = _repository!.GetChatParticipants(chatId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].GetUserId());
            Assert.AreEqual("User1", result[0].GetUsername());
            Assert.AreEqual(2, result[1].GetUserId());
            Assert.AreEqual("User2", result[1].GetUsername());
        }

        [TestMethod]
        public void GetChatParticipants_NoData_ReturnsEmptyList()
        {
            // Arrange
            int chatId = 1;
            var dataTable = new DataTable();
            _dbConnectionMock!.Setup(db => db.ExecuteReader(
                "SELECT * FROM Chat_Participants WHERE ChatID = @ChatID",
                It.Is<SqlParameter[]>(p => p[0].ParameterName == "@ChatID" && (int)p[0].Value == chatId),
                false))
                .Returns(dataTable);

            // Act
            var result = _repository!.GetChatParticipants(chatId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}