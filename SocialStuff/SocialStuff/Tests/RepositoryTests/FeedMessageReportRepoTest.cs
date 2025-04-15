using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Data.Database;
using SocialStuff.Model;
using SocialStuff.Model.MessageClasses;
using System;
using System.Collections.Generic;
using System.Data;

namespace SocialStuff.Tests.UnitTests
{
    [TestClass]
    public class RepositoryTests
    {
        private Mock<DatabaseConnection> _dbConnectionMock;
        private Repository _repository;

        [TestInitialize]
        public void Setup()
        {
            _dbConnectionMock = new Mock<DatabaseConnection>();
            _repository = new Repository();
            // Use reflection or dependency injection in real setup to inject mock
        }

        [TestMethod]
        public void GetUserById_UserExists_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("PhoneNumber", typeof(string));
            dataTable.Columns.Add("ReportedCount", typeof(int));
            dataTable.Rows.Add(userId, "user", "123", 0);
            _dbConnectionMock.Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                            .Returns(dataTable);

            // Act
            var result = _repository.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
            Assert.AreEqual("user", result.GetUsername());
        }

        [TestMethod]
        public void GetUserById_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            int userId = 1;
            var dataTable = new DataTable();
            _dbConnectionMock.Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                            .Returns(dataTable);

            // Act
            var result = _repository.GetUserById(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddTextMessage_Called_ExecutesNonQuery()
        {
            // Arrange
            int userId = 1, chatId = 2;
            string content = "Hello";

            // Act
            _repository.AddTextMessage(userId, chatId, content);

            // Assert
            _dbConnectionMock.Verify(d => d.ExecuteNonQuery("AddMessage", It.Is<SqlParameter[]>(p =>
                p[0].ParameterName == "@TypeID" && (int)p[0].Value == 1 &&
                p[1].ParameterName == "@UserID" && (int)p[1].Value == userId &&
                p[2].ParameterName == "@ChatID" && (int)p[2].Value == chatId &&
                p[3].ParameterName == "@Content" && (string)p[3].Value == content
            )), Times.Once());
        }

        [TestMethod]
        public void GetMessagesList_ReturnsMixedMessages()
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
            messagesTable.Rows.Add(1, 1, 1, 2, DateTime.Now, "Text", "", DBNull.Value, "");
            messagesTable.Rows.Add(2, 2, 1, 2, DateTime.Now, "Image.jpg", "", DBNull.Value, "");
            messagesTable.Rows.Add(3, 3, 1, 2, DateTime.Now, "Request", "Pending", 100.0f, "USD");

            var reportsTable = new DataTable();
            reportsTable.Columns.Add("messageid", typeof(int));
            _dbConnectionMock.SetupSequence(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                            .Returns(messagesTable)
                            .Returns(reportsTable);

            // Act
            var result = _repository.GetMessagesList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(TextMessage));
            Assert.IsInstanceOfType(result[1], typeof(ImageMessage));
            Assert.IsInstanceOfType(result[2], typeof(RequestMessage));
        }

        [TestMethod]
        public void GetFeedPostsList_ReturnsPosts()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("postid", typeof(int));
            dataTable.Columns.Add("title", typeof(string));
            dataTable.Columns.Add("category", typeof(string));
            dataTable.Columns.Add("content", typeof(string));
            dataTable.Columns.Add("timestamp", typeof(DateTime));
            dataTable.Rows.Add(1, "Title", "Cat", "Content", DateTime.Now);
            _dbConnectionMock.Setup(d => d.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                            .Returns(dataTable);

            // Act
            var result = _repository.GetFeedPostsList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Title", result[0].Title);
        }

        [TestMethod]
        public void AddReport_Called_ExecutesNonQuery()
        {
            // Arrange
            int messageId = 1;
            string reason = "Spam", description = "Bad", status = "Pending";

            // Act
            _repository.AddReport(messageId, reason, description, status);

            // Assert
            _dbConnectionMock.Verify(d => d.ExecuteNonQuery("AddReport", It.Is<SqlParameter[]>(p =>
                p[0].ParameterName == "@MessageID" && (int)p[0].Value == messageId &&
                p[1].ParameterName == "@Reason" && (string)p[1].Value == reason &&
                p[2].ParameterName == "@Description" && (string)p[2].Value == description &&
                p[3].ParameterName == "@Status" && (string)p[3].Value == status
            )), Times.Once());
        }
    }
}