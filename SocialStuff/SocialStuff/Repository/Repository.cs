//using System;
//using System.Collections.Generic;
//using System.Data;
//using Microsoft.Data.SqlClient;
//using SocialStuff.Database;
//using Windows.System;
//using Windows.UI.Notifications;

//namespace SocialStuff.Repository
//{
//    internal class Repository
//    {
//        private DatabaseConnection dbConnection;

//        public Repository()
//        {
//            dbConnection = new DatabaseConnection();
//        }

//        // Add a text message to the database
//        public void AddTextMessage(int senderID, int chatID, string content)
//        {
//            SqlParameter[] parameters =
//            {
//                new SqlParameter("@SenderID", senderID),
//                new SqlParameter("@ChatID", chatID),
//                new SqlParameter("@Content", content)
//            };

//            dbConnection.ExecuteNonQuery("AddTextMessage", parameters);
//        }

//        // Add an image message to the database
//        public void AddImageMessage(int senderID, int chatID, string imageURL)
//        {
//            SqlParameter[] parameters =
//            {
//                new SqlParameter("@SenderID", senderID),
//                new SqlParameter("@ChatID", chatID),
//                new SqlParameter("@ImageURL", imageURL)
//            };

//            dbConnection.ExecuteNonQuery("AddImageMessage", parameters);
//        }

//        // Add a transfer message to the database
//        public void AddTransferMessage(int senderID, int chatID, int status, float amount, string description, List<int> listOfReceivers)
//        {
//            string receivers = string.Join(",", listOfReceivers); // Convert list to string
//            SqlParameter[] parameters =
//            {
//                new SqlParameter("@SenderID", senderID),
//                new SqlParameter("@ChatID", chatID),
//                new SqlParameter("@Status", status),
//                new SqlParameter("@Amount", amount),
//                new SqlParameter("@Description", description),
//                new SqlParameter("@ListOfReceiversID", receivers) // Assuming stored procedure handles CSV format
//            };

//            dbConnection.ExecuteNonQuery("AddTransferMessage", parameters);
//        }

//        // Add a chat to the database
//        public void AddChat(List<int> participants, string chatName)
//        {
//            string participantIDs = string.Join(",", participants);
//            SqlParameter[] parameters =
//            {
//                new SqlParameter("@Participants", participantIDs),
//                new SqlParameter("@ChatName", chatName)
//            };

//            dbConnection.ExecuteNonQuery("AddChat", parameters);
//        }

//        // Get all users
//        public List<User> GetUsersList()
//        {
//            DataTable dataTable = dbConnection.ExecuteReader("GetUsersList");
//            List<User> users = new List<User>();

//            foreach (DataRow row in dataTable.Rows)
//            {
//                users.Add(new User
//                {
//                    ID = Convert.ToInt32(row["ID"]),
//                    Name = row["Name"].ToString()
//                });
//            }
//            return users;
//        }

//        // Get all notifications
//        public List<Notification> GetNotificationsList()
//        {
//            DataTable dataTable = dbConnection.ExecuteReader("GetNotificationsList");
//            List<Notification> notifications = new List<Notification>();

//            foreach (DataRow row in dataTable.Rows)
//            {
//                notifications.Add(new Notification
//                {
//                    ID = Convert.ToInt32(row["ID"]),
//                    Content = row["Content"].ToString(),
//                    UserReceiverID = Convert.ToInt32(row["UserReceiverID"])
//                });
//            }
//            return notifications;
//        }

//        // Clear all notifications
//        public void ClearAllNotifications()
//        {
//            dbConnection.ExecuteNonQuery("ClearAllNotifications");
//        }

//        // Update chats
//        public void UpdateChats()
//        {
//            dbConnection.ExecuteNonQuery("UpdateChats");
//        }

//        // Update users
//        public void UpdateUsers()
//        {
//            dbConnection.ExecuteNonQuery("UpdateUsers");
//        }

//        // Update notifications
//        public void UpdateNotifications()
//        {
//            dbConnection.ExecuteNonQuery("UpdateNotifications");
//        }

//        // Add a report message
//        public void AddReportMessage(int messageID, int reporterUserID, int status, string reason, string description)
//        {
//            SqlParameter[] parameters =
//            {
//                new SqlParameter("@MessageID", messageID),
//                new SqlParameter("@ReporterUserID", reporterUserID),
//                new SqlParameter("@Status", status),
//                new SqlParameter("@Reason", reason),
//                new SqlParameter("@Description", description)
//            };

//            dbConnection.ExecuteNonQuery("AddReportMessage", parameters);
//        }

//        // Update reports
//        public void UpdateReports()
//        {
//            dbConnection.ExecuteNonQuery("UpdateReports");
//        }
//    }
//}
