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
using Moq;
using Microsoft.Data.SqlClient;

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
            _repository = new Repository();
            // Use reflection to set private dbConnection field for testing
            typeof(Repository).GetField("dbConnection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_repository, _dbConnectionMock.Object);
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
            var dataTable = new DataTable();
            dataTable.Columns.Add("userid", typeof(int));
            dataTable.Columns.Add("username", typeof(string));
            dataTable.Columns.Add("phonenumber", typeof(string));
            dataTable.Columns.Add("reportedcount", typeof(int));
            dataTable.Rows.Add(1, "User1", "1234567890", 0);
            dataTable.Rows.Add(2, "User2", "0987654321", 1);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Users", null, false))
                .Returns(dataTable);

            var result = _repository.GetUsersList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("User1", result[0].GetUsername());
            Assert.AreEqual("User2", result[1].GetUsername());
        }

        [TestMethod]
        public void GetUserFriendsList_ReturnsFriends()
        {
            int userId = 1;
            var usersTable = new DataTable();
            usersTable.Columns.Add("userid", typeof(int));
            usersTable.Columns.Add("username", typeof(string));
            usersTable.Columns.Add("phonenumber", typeof(string));
            usersTable.Columns.Add("reportedcount", typeof(int));
            usersTable.Rows.Add(2, "Friend1", "1234567890", 0);
            usersTable.Rows.Add(3, "Friend2", "0987654321", 0);
            var friendsTable = new DataTable();
            friendsTable.Columns.Add("userid", typeof(int));
            friendsTable.Columns.Add("friendid", typeof(int));
            friendsTable.Rows.Add(userId, 2);
            friendsTable.Rows.Add(userId, 3);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Users", null, false))
                .Returns(usersTable);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Friends", null, false))
                .Returns(friendsTable);

            var result = _repository.GetUserFriendsList(userId);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].GetUserId());
            Assert.AreEqual(3, result[1].GetUserId());
        }

        [TestMethod]
        public void GetFriendsIDs_ReturnsFriendIds()
        {
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("userid", typeof(int));
            dataTable.Columns.Add("friendid", typeof(int));
            dataTable.Rows.Add(userId, 2);
            dataTable.Rows.Add(userId, 3);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from users", null, false))
                .Returns(new DataTable());
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Friends", null, false))
                .Returns(dataTable);

            var result = _repository.GetFriendsIDs(userId);

            CollectionAssert.AreEqual(new List<int> { 2, 3 }, result);
        }

        [TestMethod]
        public void GetChatsList_ReturnsAllChats()
        {
            var chatsTable = new DataTable();
            chatsTable.Columns.Add("chatid", typeof(int));
            chatsTable.Columns.Add("chatname", typeof(string));
            chatsTable.Rows.Add(100, "Chat1");
            var participantsTable = new DataTable();
            participantsTable.Columns.Add("chatid", typeof(int));
            participantsTable.Columns.Add("userid", typeof(int));
            participantsTable.Rows.Add(100, 1);
            participantsTable.Rows.Add(100, 2);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Chats", null, false))
                .Returns(chatsTable);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from Chat_Participants", null, false))
                .Returns(participantsTable);

            var result = _repository.GetChatsList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].getChatID());
            CollectionAssert.AreEqual(new List<int> { 1, 2 }, result[0].getUserIDsList());
        }

        [TestMethod]
        public void GetChatsIDs_ReturnsChatIds()
        {
            int userId = 1;
            var dataTable = new DataTable();
            dataTable.Columns.Add("ChatID", typeof(int));
            dataTable.Rows.Add(100);
            dataTable.Rows.Add(200);
            _dbConnectionMock.Setup(db => db.ExecuteReader("select * from chats", null, false))
                .Returns(dataTable);

            var result = _repository.GetChatsIDs(userId);

            CollectionAssert.AreEqual(new List<int> { 100, 200 }, result);
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

        // Integration test example (assuming a real database connection for integration)
        [TestMethod]
        [Ignore] // Ignore unless running against actual DB
        public void GetUserById_Integration_WithRealDatabase()
        {
            var repository = new Repository(); // Real DB connection
            int userId = 1; // Assumes user with ID 1 exists in test DB

            var result = repository.GetUserById(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
        }
    }
}
