using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialStuff.Data;
using SocialStuff.Data.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace SocialStuff.Tests.RepositoryTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<DatabaseConnection> _dbConnectionMock;
        private Repository _repository;

        [TestInitialize]
        public void Setup()
        {
            _dbConnectionMock = new Mock<DatabaseConnection>();
            _repository = new Repository(_dbConnectionMock.Object);
        }

        [TestMethod]
        public void GetLoggedInUserID_ReturnsCorrectId()
        {
            var result = _repository.GetLoggedInUserID();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetUserById_ExistingUser_ReturnsUser()
        {
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("PhoneNumber", typeof(string));
            dataTable.Columns.Add("ReportedCount", typeof(int));
            dataTable.Rows.Add(userId, "User1", "1234567890", 0);
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            var result = _repository.GetUserById(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
            Assert.AreEqual("User1", result.GetUsername());
        }

        [TestMethod]
        public void GetUsersList_ReturnsAllUsers()
        {
            // rescrie
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetUserFriendsList_ReturnsFriends()
        {
            // rescrie
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetFriendsIDs_ReturnsFriendIds()
        {
            // rescrie
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetChatsList_ReturnsAllChats()
        {
            // rescrie
            Assert.Fail("Not implemented yet.");
        }

        [TestMethod]
        public void GetChatsIDs_ReturnsChatIds()
        {
            // rescrie
            Assert.Fail("Not implemented yet.");
        }


        [TestMethod]
        public void AddFriend_CallsExecuteNonQuery()
        {
            int userId = 1;
            int friendId = 2;

            _repository.AddFriend(userId, friendId);

            _dbConnectionMock.Verify(db => db.ExecuteNonQuery("AddFriend", It.IsAny<SqlParameter[]>()), Times.Once());
        }

        [TestMethod]
        public void DeleteFriend_CallsExecuteNonQuery()
        {
            int userId = 1;
            int friendId = 2;

            _repository.DeleteFriend(userId, friendId);

            _dbConnectionMock.Verify(db => db.ExecuteNonQuery("DeleteFriend", It.IsAny<SqlParameter[]>()), Times.Once());
        }

        [TestMethod]
        public void AddUserToChat_CallsExecuteNonQuery()
        {
            int userId = 1;
            int chatId = 100;

            _repository.AddUserToChat(userId, chatId);

            _dbConnectionMock.Verify(db => db.ExecuteNonQuery("AddUserToChat", It.IsAny<SqlParameter[]>()), Times.Once());
        }

        [TestMethod]
        public void RemoveUserFromChat_CallsExecuteNonQuery()
        {
            int userId = 1;
            int chatId = 100;

            _repository.RemoveUserFromChat(userId, chatId);

            _dbConnectionMock.Verify(db => db.ExecuteNonQuery("RemoveUserFromChat", It.IsAny<SqlParameter[]>()), Times.Once());
        }
    }
}
