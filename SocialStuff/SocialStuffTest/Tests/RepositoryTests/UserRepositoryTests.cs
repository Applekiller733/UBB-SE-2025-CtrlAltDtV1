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
        private Mock<DatabaseConnection>? _dbConnectionMock;
        private Repository? _repository;

        [TestInitialize]
        public void Setup()
        {
            _dbConnectionMock = new Mock<DatabaseConnection>();
            _repository = new Repository(_dbConnectionMock.Object);
        }

        [TestMethod]
        public void GetLoggedInUserID_ReturnsCorrectId()
        {
            var result = _repository!.GetLoggedInUserID();

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
            _dbConnectionMock!.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            var result = _repository!.GetUserById(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.GetUserId());
            Assert.AreEqual("User1", result.GetUsername());
        }

        [TestMethod]
        public void GetUsersList_ReturnsAllUsers()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("userid", typeof(int));
            dataTable.Columns.Add("username", typeof(string));
            dataTable.Columns.Add("phonenumber", typeof(string));
            dataTable.Columns.Add("reportedcount", typeof(int));

            // Add mock data for the test
            dataTable.Rows.Add(1, "User1", "1234567890", 0);
            dataTable.Rows.Add(2, "User2", "0987654321", 1);

            // Set up the mock to return the DataTable when ExecuteReader is called
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetUsersList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Check that two users were returned

            var firstUser = result.First();
            Assert.AreEqual(1, firstUser.GetUserId());
            Assert.AreEqual("User1", firstUser.GetUsername());
            Assert.AreEqual("1234567890", firstUser.GetPhoneNumber());
            Assert.AreEqual(0, firstUser.GetReportedCount());

            var secondUser = result.Last();
            Assert.AreEqual(2, secondUser.GetUserId());
            Assert.AreEqual("User2", secondUser.GetUsername());
            Assert.AreEqual("0987654321", secondUser.GetPhoneNumber());
            Assert.AreEqual(1, secondUser.GetReportedCount());
        }

        [TestMethod]
        public void GetUserFriendsList_ReturnsFriends()
        {
            // Arrange
            var userDataTable = new DataTable();
            userDataTable.Columns.Add("userid", typeof(int));
            userDataTable.Columns.Add("username", typeof(string));
            userDataTable.Columns.Add("phonenumber", typeof(string));
            userDataTable.Columns.Add("reportedcount", typeof(int));  // Add this for the User class constructor

            // Add mock users (for friends of user 1)
            userDataTable.Rows.Add(1, "User1", "123-456-7890", 0); // User 1
            userDataTable.Rows.Add(2, "User2", "234-567-8901", 0); // User 2
            userDataTable.Rows.Add(3, "User3", "345-678-9012", 0); // User 3

            // Arrange Friends table (for user 1)
            var friendsDataTable = new DataTable();
            friendsDataTable.Columns.Add("userid", typeof(int));
            friendsDataTable.Columns.Add("friendid", typeof(int));  // This should hold friend IDs

            // User 1 has friends 2 and 3
            friendsDataTable.Rows.Add(1, 2); // User 1 has friend 2
            friendsDataTable.Rows.Add(1, 3); // User 1 has friend 3

            // Mock the ExecuteReader method to return the users and friends data
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.Is<string>(s => s.Contains("Users")), It.IsAny<SqlParameter[]>(), false))
                .Returns(userDataTable);

            _dbConnectionMock.Setup(db => db.ExecuteReader(It.Is<string>(s => s.Contains("Friends")), It.IsAny<SqlParameter[]>(), false))
                .Returns(friendsDataTable);

            // Act
            var result = _repository.GetUserFriendsList(1); // Assuming user ID 1 is the subject

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // User 1 should have 2 friends

            // Check the friends returned
            var friend1 = result.FirstOrDefault(u => u.GetUserId() == 2);
            var friend2 = result.FirstOrDefault(u => u.GetUserId() == 3);

            Assert.IsNotNull(friend1); // User 2 should be in the list
            Assert.IsNotNull(friend2); // User 3 should be in the list
        }



        [TestMethod]
        public void GetFriendsIDs_ReturnsFriendIds()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("friendid", typeof(int));  // Ensure 'friendid' exists in the result

            // Add mock data
            dataTable.Rows.Add(2);  // Friend ID 2
            dataTable.Rows.Add(3);  // Friend ID 3

            // Mock the ExecuteReader method to return our dataTable
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetFriendsIDs(1);  // Assuming user ID 1 is the test subject

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);  // Expecting 2 friend IDs

            // Check friend IDs
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);
        }

        [TestMethod]
        public void GetChatsList_ReturnsAllChats()
        {
            // Arrange
            var chatsDataTable = new DataTable();
            chatsDataTable.Columns.Add("chatid", typeof(int));
            chatsDataTable.Columns.Add("chatname", typeof(string));

            // Add mock data for the chats table
            chatsDataTable.Rows.Add(1, "Chat 1");
            chatsDataTable.Rows.Add(2, "Chat 2");

            var participantsDataTable = new DataTable();
            participantsDataTable.Columns.Add("chatid", typeof(int));
            participantsDataTable.Columns.Add("userid", typeof(int));

            // Add mock data for the chat participants table
            participantsDataTable.Rows.Add(1, 1); // Chat 1 has user 1
            participantsDataTable.Rows.Add(1, 2); // Chat 1 has user 2
            participantsDataTable.Rows.Add(2, 1); // Chat 2 has user 1

            // Mock the ExecuteReader method to return the chats and participants data
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.Is<string>(s => s.Contains("Chats")), It.IsAny<SqlParameter[]>(), false))
                .Returns(chatsDataTable);

            _dbConnectionMock.Setup(db => db.ExecuteReader(It.Is<string>(s => s.Contains("Chat_Participants")), It.IsAny<SqlParameter[]>(), false))
                .Returns(participantsDataTable);

            // Act
            var result = _repository.GetChatsList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Should have two chats

            // Check that Chat 1 has two participants: 1 and 2
            var chat1 = result.FirstOrDefault(c => c.getChatID() == 1);
            Assert.IsNotNull(chat1);
            Assert.AreEqual(2, chat1.getUserIDsList().Count); // Chat 1 has two participants
            Assert.IsTrue(chat1.getUserIDsList().Contains(1));
            Assert.IsTrue(chat1.getUserIDsList().Contains(2));

            // Check that Chat 2 has one participant: 1
            var chat2 = result.FirstOrDefault(c => c.getChatID() == 2);
            Assert.IsNotNull(chat2);
            Assert.AreEqual(1, chat2.getUserIDsList().Count); // Chat 2 has one participant
            Assert.IsTrue(chat2.getUserIDsList().Contains(1));
        }



        [TestMethod]
        public void GetChatsIDs_ReturnsChatIds()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("chatid", typeof(int));  // Assuming this is a list of chat IDs

            // Add mock data for the test
            dataTable.Rows.Add(101);  // User's first chat ID
            dataTable.Rows.Add(102);  // User's second chat ID

            // Mock the ExecuteReader method to return the chat IDs
            _dbConnectionMock.Setup(db => db.ExecuteReader(It.IsAny<string>(), It.IsAny<SqlParameter[]>(), false))
                .Returns(dataTable);

            // Act
            var result = _repository.GetChatsIDs(1); // Assuming user ID 1 is the subject

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Should return two chat IDs

            // Check the first chat ID
            Assert.AreEqual(101, result.First());

            // Check the second chat ID
            Assert.AreEqual(102, result.Last());
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
