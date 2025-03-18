using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SocialStuff.Data.Database;
using User = SocialStuff.Model.User;
using Windows.System;
using Windows.UI.Notifications;

namespace SocialStuff.Data
{
    internal class Repository
    {
        private DatabaseConnection dbConnection;
        private static int loggedInUserID=1;

        public Repository()
        {
            dbConnection = new DatabaseConnection();
            Console.WriteLine("Repo created");
        }


        public DatabaseConnection GetDatabaseConnection()
        {
            return dbConnection;
        }

        public static int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        // Get all users
        public List<User> GetUsersList()
        {
            DataTable dataTable = dbConnection.ExecuteReader("GetUsersList");
            List<User> users = new List<User>();

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new User
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Name = row["Name"].ToString()
                });
            }
            return users;
        }

        //Get all notifications
        public List<Notification> GetNotificationsList()
        {
            DataTable dataTable = dbConnection.ExecuteReader("GetNotificationsList");
            List<Notification> notifications = new List<Notification>();

            foreach (DataRow row in dataTable.Rows)
            {
                notifications.Add(new Notification
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Content = row["Content"].ToString(),
                    UserReceiverID = Convert.ToInt32(row["UserReceiverID"])
                });
            }
            return notifications;
        }


        // Add a chat to the database
        public void AddChat(string chatName, out int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatName", chatName),
                new SqlParameter("@ChatID", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            dbConnection.ExecuteNonQuery("AddChat", parameters);
            chatID = (int)parameters[1].Value; // Get the generated ChatID from the output parameter
        }

        // Update a chat in the database
        public void UpdateChat(int chatID, string chatName)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@ChatName", chatName)
            };

            dbConnection.ExecuteNonQuery("UpdateChat", parameters);
        }

        // Delete a chat from the database
        public void DeleteChat(int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatID", chatID)
            };

            dbConnection.ExecuteNonQuery("DeleteChat", parameters);
        }

        // Add a friend to the database
        public void AddFriend(int userID, int friendID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@FriendID", friendID)
            };

            dbConnection.ExecuteNonQuery("AddFriend", parameters);
        }

        // Delete a friend from the database
        public void DeleteFriend(int userID, int friendID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@FriendID", friendID)
            };

            dbConnection.ExecuteNonQuery("DeleteFriend", parameters);
        }

        // Add a user to the database
        public void AddUser(string username, string phoneNumber)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@PhoneNumber", phoneNumber)
            };

            dbConnection.ExecuteNonQuery("AddUser", parameters);
        }

        // Update a user in the database
        public void UpdateUser(int userID, string username, string phoneNumber)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@Username", username),
                new SqlParameter("@PhoneNumber", phoneNumber)
            };

            dbConnection.ExecuteNonQuery("UpdateUser", parameters);
        }

        // Delete a user from the database
        public void DeleteUser(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID)
            };

            dbConnection.ExecuteNonQuery("DeleteUser", parameters);
        }

        // Add a message to the database (handles all types)
        public void AddTextMessage(int userID, int chatID, string content)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 1),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content)
            };

            dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        public void AddImageMessage(int userID, int chatID, string ImageURL)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 2),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", ImageURL)
            };

            dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }


        public void AddRequestMessage(int userID, int chatID, string content, string status = null, decimal? amount = null, string currency = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 3),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content),
                new SqlParameter("@Status", status ?? (object)DBNull.Value),
                new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
                new SqlParameter("@Currency", currency ?? (object)DBNull.Value)
            };

            dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        public void AddTransferMessage(int userID, int chatID, string content, string status = null, decimal? amount = null, string currency = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 4),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content),
                new SqlParameter("@Status", status ?? (object)DBNull.Value),
                new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
                new SqlParameter("@Currency", currency ?? (object)DBNull.Value)
            };

            dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }


        // Update a message in the database
        public void UpdateMessage(int messageID, int typeID, string content, string status = null, decimal? amount = null, string currency = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageID", messageID),
                new SqlParameter("@TypeID", typeID),
                new SqlParameter("@Content", content),
                new SqlParameter("@Status", status ?? (object)DBNull.Value),
                new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
                new SqlParameter("@Currency", currency ?? (object)DBNull.Value)
            };

            dbConnection.ExecuteNonQuery("UpdateMessage", parameters);
        }

        // Delete a message from the database
        public void DeleteMessage(int messageID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageID", messageID)
            };

            dbConnection.ExecuteNonQuery("DeleteMessage", parameters);
        }

        // Add a notification
        public void AddNotification(string content, int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Content", content),
                new SqlParameter("@UserID", userID)
            };

            dbConnection.ExecuteNonQuery("AddNotification", parameters);
        }

        // Delete a notification
        public void DeleteNotification(int notifID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@NotifID", notifID)
            };

            dbConnection.ExecuteNonQuery("DeleteNotification", parameters);
        }

        // Clear all notifications
        public void ClearAllNotifications()
        {
            dbConnection.ExecuteNonQuery("DeleteAllNotifications");
        }

        // Add a report
        public void AddReport(int messageID, string reason, string description, string status)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageID", messageID),
                new SqlParameter("@Reason", reason),
                new SqlParameter("@Description", description ?? (object)DBNull.Value),
                new SqlParameter("@Status", status)
            };

            dbConnection.ExecuteNonQuery("AddReport", parameters);
        }

        // Update a report
        public void UpdateReport(int reportID, string reason, string description, string status)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ReportID", reportID),
                new SqlParameter("@Reason", reason),
                new SqlParameter("@Description", description ?? (object)DBNull.Value),
                new SqlParameter("@Status", status)
            };

            dbConnection.ExecuteNonQuery("UpdateReport", parameters);
        }

        // Delete a report
        public void DeleteReport(int reportID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ReportID", reportID)
            };

            dbConnection.ExecuteNonQuery("DeleteReport", parameters);
        }

        // Add a feed post
        public void AddFeedPost(string title, string category, string content)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Title", title),
                new SqlParameter("@Category", category),
                new SqlParameter("@Content", content)
            };

            dbConnection.ExecuteNonQuery("AddFeedPost", parameters);
        }

        // Update a feed post
        public void UpdateFeedPost(int postID, string title, string category, string content)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@PostID", postID),
                new SqlParameter("@Title", title),
                new SqlParameter("@Category", category),
                new SqlParameter("@Content", content)
            };

            dbConnection.ExecuteNonQuery("UpdateFeedPost", parameters);
        }

        // Delete a feed post
        public void DeleteFeedPost(int postID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@PostID", postID)
            };

            dbConnection.ExecuteNonQuery("DeleteFeedPost", parameters);
        }

        public void AddUserToChat(int userID, int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID)
            };
            dbConnection.ExecuteNonQuery("AddUserToChat", parameters);
        }

        public void RemoveUserFromChat(int userID, int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID)
            };
            dbConnection.ExecuteNonQuery("RemoveUserFromChat", parameters);
        }

        internal int getLoggedInUser()
        {
            throw new NotImplementedException();
        }
    }

}

