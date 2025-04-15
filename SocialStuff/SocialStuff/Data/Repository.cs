// <copyright file="Repository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using SocialStuff.Data.Database;
    using SocialStuff.Model;
    using SocialStuff.Model.MessageClasses;
    using Windows.System;
    using Windows.UI.Notifications;
    using NotificationModel = SocialStuff.Model.Notification;
    using User = SocialStuff.Model.User;
    using WindowsNotification = Windows.UI.Notifications.Notification;

    public class Repository : IRepository
    {
        public const int LOGGEDINUSERID = 2;
        private static int loggedInUserID = LOGGEDINUSERID;
        private DatabaseConnection dbConnection;

        public Repository()
        {
            this.dbConnection = new DatabaseConnection();
            Console.WriteLine("Repo created");
        }

        // public DatabaseConnection GetDatabaseConnection()
        // {
        //    return dbConnection;
        // }
        public int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        public User GetUserById(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
            };
            DataTable dataTable = this.dbConnection.ExecuteReader("SELECT * FROM Users WHERE UserID = @UserID", parameters, false);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int id = Convert.ToInt32(row["UserID"]);
                string username = row["Username"]?.ToString() ?? string.Empty;
                string phoneNumber = row["PhoneNumber"]?.ToString() ?? string.Empty;
                int reportedCount = Convert.ToInt32(row["ReportedCount"]);
                return new User(id, username, phoneNumber, reportedCount);
            }

            return new User();
        }

        // Get a chat by ID
        public Chat? GetChatById(int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatID", chatID),
            };
            DataTable dataTable = this.dbConnection.ExecuteReader("SELECT * FROM Chats WHERE ChatID = @ChatID", parameters, false);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                int id = Convert.ToInt32(row["ChatID"]);
                string chatName = row["ChatName"]?.ToString() ?? string.Empty;
                List<int> participants = this.GetChatParticipants(id).ConvertAll(p => p.GetUserId());
                return new Chat(id, chatName, participants);
            }

            return null;
        }

        // Get all users
        public List<User> GetUsersList()
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Users", Array.Empty<SqlParameter>(), false);
            List<User> users = new List<User>();

            foreach (DataRow row in dataTable.Rows)
            {
                int userID = Convert.ToInt32(row["userid"]);
                string username = row["username"]?.ToString() ?? string.Empty;
                string phoneNumber = row["phonenumber"]?.ToString() ?? string.Empty;
                int reportedCount = Convert.ToInt32(row["reportedcount"]);
                users.Add(new User(userID, username, phoneNumber, reportedCount));
            }

            return users;
        }

        // Get notifications for a specific user
        public List<NotificationModel> GetNotifications(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
            };
            DataTable dataTable = this.dbConnection.ExecuteReader("SELECT * FROM Notifications WHERE UserID = @UserID ORDER BY Timestamp DESC", parameters, false);
            List<NotificationModel> notifications = new List<NotificationModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                int notificationID = Convert.ToInt32(row["NotifID"]);
                DateTime timestamp = Convert.ToDateTime(row["Timestamp"]);
                string content = row["Content"]?.ToString() ?? string.Empty;
                int userReceiverID = Convert.ToInt32(row["UserID"]);
                notifications.Add(new NotificationModel(notificationID, timestamp, content, userReceiverID));
            }

            return notifications;
        }

        // Get the Friends of a USERID, friends returned as User Class Type
        public List<User> GetUserFriendsList(int userId)
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Users", Array.Empty<SqlParameter>(), false);
            DataTable dataTable1 = this.dbConnection.ExecuteReader("select * from Friends", Array.Empty<SqlParameter>(), false);
            List<int> friendIds = new List<int>();
            foreach (DataRow row in dataTable1.Rows)
            {
                if (Convert.ToInt32(row["userid"]) == userId)
                {
                    friendIds.Add(Convert.ToInt32(row["friendid"]));
                }
            }

            List<User> users = new List<User>();
            foreach (DataRow row in dataTable.Rows)
            {
                int userID = Convert.ToInt32(row["userid"]);
                if (friendIds.Contains(userID))
                {
                    string username = row["username"]?.ToString() ?? string.Empty;
                    string phoneNumber = row["phonenumber"]?.ToString() ?? string.Empty;
                    int reportedCount = Convert.ToInt32(row["reportedcount"]);
                    users.Add(new User(userID, username, phoneNumber, reportedCount));
                }
            }

            return users;
        }
        //// Get all the Chats for a USERID, chats returned as Chat Class Type
        // public List<Chat> GetUserChatsList(int userId)
        // {
        //    DataTable dataTable = dbConnection.ExecuteReader("select * from Chats", null, false);
        //    DataTable dataTable1 = dbConnection.ExecuteReader("select * from ChatParticipants", null, false);
        //    List<int> ChatIds = new List<int>();
        //    foreach (DataRow row in dataTable1.Rows)
        //    {
        //        if (Convert.ToInt32(row["userid"]) == userId)
        //        {
        //            ChatIds.Add(Convert.ToInt32(row["chatid"]));
        //        }
        //    }
        //    List<Chat> Chats = new List<Chat>();
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        int chat = Convert.ToInt32(row["chatid"]);
        //        if (ChatIds.Contains(chat))
        //        {
        //            string chatName = row["chatname"].ToString();
        //            List<int> ParticipantsIDs = new List<int>();
        //            foreach (DataRow row1 in dataTable1.Rows)
        //            {
        //                if (chat == Convert.ToInt32(row1["chatid"]))
        //                {
        //                    ParticipantsIDs.Add(Convert.ToInt32(row1["userid"]));
        //                }
        //            }
        //            Chats.Add(new Chat(chat, chatName, ParticipantsIDs));
        // }
        //    }
        //    return Chats;
        // }

        // Get all chats
        public List<Chat> GetChatsList()
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Chats", Array.Empty<SqlParameter>(), false);
            DataTable dataTable1 = this.dbConnection.ExecuteReader("select * from Chat_Participants", Array.Empty<SqlParameter>(), false);
            List<Chat> chats = new List<Chat>();
            foreach (DataRow row in dataTable.Rows)
            {
                int chatID = Convert.ToInt32(row["chatid"]);
                string chatName = row["chatname"]?.ToString() ?? string.Empty;
                List<int> participantsIDs = new List<int>();
                foreach (DataRow row1 in dataTable1.Rows)
                {
                    if (chatID == Convert.ToInt32(row1["chatid"]))
                    {
                        participantsIDs.Add(Convert.ToInt32(row1["userid"]));
                    }
                }

                chats.Add(new Chat(chatID, chatName, participantsIDs));
            }

            return chats;
        }

        // get the pparticipants of a chat
        public List<User> GetChatParticipants(int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatID", chatID),
            };
            DataTable dataTable = this.dbConnection.ExecuteReader("SELECT * FROM Chat_Participants WHERE ChatID = @ChatID", parameters, false);
            List<int> userIds = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                userIds.Add(Convert.ToInt32(row["UserID"]));
            }

            List<User> users = new List<User>();
            foreach (int userID in userIds)
            {
                users.Add(this.GetUserById(userID));
            }

            return users;
        }

        // <summary>
        // Gets the list of all messages.
        // </summary>
        // <returns>A list of messages.</returns>
        public List<Message> GetMessagesList()
        {
            // messagetypes : 1-text , 2-image, 3-request, 4-transfer
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Messages", Array.Empty<SqlParameter>(), false);
            List<Message> messages = new List<Message>();
            foreach (DataRow row in dataTable.Rows)
            {
                int messageID = Convert.ToInt32(row["messageid"]);
                int typeID = Convert.ToInt32(row["typeid"]);
                int userID = Convert.ToInt32(row["userid"]);
                int chatID = Convert.ToInt32(row["chatid"]);
                DateTime timestamp = Convert.ToDateTime(row["timestamp"]);
                string content = row["content"]?.ToString() ?? string.Empty;
                string status = row["status"]?.ToString() ?? string.Empty;
                float? amount = row["amount"] == DBNull.Value ? (float?)null : Convert.ToSingle(row["amount"]);
                string currency = row["currency"]?.ToString() ?? string.Empty;
                DataTable reportsTable = this.dbConnection.ExecuteReader("select * from Reports", Array.Empty<SqlParameter>(), false);
                List<int> userReports = new List<int>();
                foreach (DataRow row1 in reportsTable.Rows)
                {
                    if (Convert.ToInt32(row1["messageid"]) == messageID)
                    {
                        userReports.Add(userID);
                    }
                }

                switch (typeID)
                {
                    case 1: // Text message
                        messages.Add(new TextMessage(messageID, userID, chatID, timestamp, content, userReports));
                        break;
                    case 2: // Image message
                        messages.Add(new ImageMessage(messageID, userID, chatID, timestamp, content, userReports));
                        break;
                    case 3: // Request message
                        messages.Add(new RequestMessage(messageID, userID, chatID, timestamp, status, amount ?? 0, content, currency));
                        break;
                    case 4: // Transfer message
                        messages.Add(new TransferMessage(messageID, userID, chatID, timestamp, status, amount ?? 0, content, currency));
                        break;
                    default:
                        throw new Exception("Unknown message type");
                }
            }

            return messages;
        }

        // Get all friends ids of a user
        public List<int> GetFriendsIDs(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
            };
            DataTable users = this.dbConnection.ExecuteReader("select * from users", Array.Empty<SqlParameter>(), false);
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Friends", Array.Empty<SqlParameter>(), false);
            List<int> friends = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToInt32(row["userid"]) == userID)
                {
                    friends.Add(Convert.ToInt32(row["friendid"]));
                }
            }

            return friends;
        }

        // Get all reports
        public List<Report> GetReportsList()
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Reports", Array.Empty<SqlParameter>(), false);
            List<Report> reports = new List<Report>();
            foreach (DataRow row in dataTable.Rows)
            {
                int reportID = Convert.ToInt32(row["reportid"]);
                int messageID = Convert.ToInt32(row["messageid"]);
                string reason = row["reason"]?.ToString() ?? string.Empty;
                string description = row["description"]?.ToString() ?? string.Empty;
                string status = row["status"]?.ToString() ?? string.Empty;

                // reports.Add(new Report(reportID, messageID, reason, description, status));
            }

            return reports;
        }

        // Get all feed posts
        public List<Post> GetFeedPostsList()
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Feed_Posts", Array.Empty<SqlParameter>(), false);
            List<Post> feedPosts = new List<Post>();
            foreach (DataRow row in dataTable.Rows)
            {
                int postID = Convert.ToInt32(row["postid"]);
                string title = row["title"]?.ToString() ?? string.Empty;
                string category = row["category"]?.ToString() ?? string.Empty;
                string content = row["content"]?.ToString() ?? string.Empty;
                DateTime timestamp = Convert.ToDateTime(row["timestamp"]);
                feedPosts.Add(new Post(postID, title, category, content, timestamp));
            }

            return feedPosts;
        }

        // Get all chats ids of a user
        public List<int> GetChatsIDs(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
            };
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from chats", Array.Empty<SqlParameter>(), false);
            List<int> chats = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                chats.Add(Convert.ToInt32(row["ChatID"]));
            }

            return chats;
        }

        // Add a chat to the database
        public int AddChat(string chatName)
        {
            int chatID;
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatName", chatName),
                new SqlParameter("@ChatID", SqlDbType.Int) { Direction = ParameterDirection.Output },
            };

            this.dbConnection.ExecuteNonQuery("AddChat", parameters);
            chatID = (int)parameters[1].Value; // Get the generated ChatID from the output parameter
            return chatID;
        }

        public List<int> GetChatParticipantsIDs(int chatID)
        {
            DataTable dataTable = this.dbConnection.ExecuteReader("select * from Chat_Participants", Array.Empty<SqlParameter>(), false);
            List<int> participants = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToInt32(row["chatid"]) == chatID)
                {
                    participants.Add(Convert.ToInt32(row["userid"]));
                }
            }

            return participants;
        }

        //// Update a chat in the database
        // public void UpdateChat(int chatID, string chatName)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@ChatID", chatID),
        //        new SqlParameter("@ChatName", chatName)
        //    };
        // dbConnection.ExecuteNonQuery("UpdateChat", parameters);
        // }

        // Delete a chat from the database
        public void DeleteChat(int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ChatID", chatID),
            };

            this.dbConnection.ExecuteNonQuery("DeleteChat", parameters);
        }

        // Add a friend to the database
        public void AddFriend(int userID, int friendID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@FriendID", friendID),
            };

            this.dbConnection.ExecuteNonQuery("AddFriend", parameters);
        }

        // Delete a friend from the database
        public void DeleteFriend(int userID, int friendID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@FriendID", friendID),
            };

            this.dbConnection.ExecuteNonQuery("DeleteFriend", parameters);
        }

        //// Add a user to the database
        // public void AddUser(string username, string phoneNumber)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Username", username),
        //        new SqlParameter("@PhoneNumber", phoneNumber)
        //    };

        // dbConnection.ExecuteNonQuery("AddUser", parameters);
        // }

        //// Update a user in the database
        // public void UpdateUser(int userID, string username, string phoneNumber)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@UserID", userID),
        //        new SqlParameter("@Username", username),
        //        new SqlParameter("@PhoneNumber", phoneNumber)
        //    };

        // dbConnection.ExecuteNonQuery("UpdateUser", parameters);
        // }

        //// Delete a user from the database
        // public void DeleteUser(int userID)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@UserID", userID)
        //    };

        // dbConnection.ExecuteNonQuery("DeleteUser", parameters);
        // }

        // Add a message to the database (handles all types)
        public void AddTextMessage(int userID, int chatID, string content)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 1),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content),
            };

            this.dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        public void AddImageMessage(int userID, int chatID, string imageURL)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 2),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", imageURL),
            };

            this.dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        public void AddRequestMessage(int userID, int chatID, string content, string? status = null, float? amount = null, string? currency = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 3),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content),
                new SqlParameter("@Status", status ?? (object)DBNull.Value),
                new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
                new SqlParameter("@Currency", currency ?? (object)DBNull.Value),
            };

            this.dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        public void AddTransferMessage(int userID, int chatID, string content, string? status = null, float? amount = null, string? currency = null)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeID", 4),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
                new SqlParameter("@Content", content),
                new SqlParameter("@Status", status ?? (object)DBNull.Value),
                new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
                new SqlParameter("@Currency", currency ?? (object)DBNull.Value),
            };

            this.dbConnection.ExecuteNonQuery("AddMessage", parameters);
        }

        //// Update a message in the database
        // public void UpdateMessage(int messageID, int typeID, string content, string status = null, float? amount = null, string currency = null)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@MessageID", messageID),
        //        new SqlParameter("@TypeID", typeID),
        //        new SqlParameter("@Content", content),
        //        new SqlParameter("@Status", status ?? (object)DBNull.Value),
        //        new SqlParameter("@Amount", amount ?? (object)DBNull.Value),
        //        new SqlParameter("@Currency", currency ?? (object)DBNull.Value)
        //    };

        // dbConnection.ExecuteNonQuery("UpdateMessage", parameters);
        // }

        // Delete a message from the database
        public void DeleteMessage(int messageID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageID", messageID),
            };

            this.dbConnection.ExecuteNonQuery("DeleteMessage", parameters);
        }

        // Add a notification
        public void AddNotification(string content, int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@Content", content),
                new SqlParameter("@UserID", userID),
            };

            this.dbConnection.ExecuteNonQuery("AddNotification", parameters);
        }

        // Delete a notification
        public void DeleteNotification(int notifID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@NotifID", notifID),
            };

            this.dbConnection.ExecuteNonQuery("DeleteNotification", parameters);
        }

        // Clear all notifications
        public void ClearAllNotifications(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
            };

            this.dbConnection.ExecuteNonQuery("DeleteAllNotifications", parameters);
        }

        // <summary>
        // Adds a report to the database.
        // </summary>
        // <param name="messageID">The ID of the message being reported.</param>
        // <param name="reason">The reason for the report.</param>
        // <param name="description">The description of the report.</param>
        // <param name="status">The status of the report.</param>
        public void AddReport(int messageID, string reason, string description, string status)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MessageID", messageID),
                new SqlParameter("@Reason", reason),
                new SqlParameter("@Description", description ?? (object)DBNull.Value),
                new SqlParameter("@Status", status),
            };

            this.dbConnection.ExecuteNonQuery("AddReport", parameters);
        }

        //// Update a report
        // public void UpdateReport(int reportID, string reason, string description, string status)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@ReportID", reportID),
        //        new SqlParameter("@Reason", reason),
        //        new SqlParameter("@Description", description ?? (object)DBNull.Value),
        //        new SqlParameter("@Status", status)
        //    };

        // sdbConnection.ExecuteNonQuery("UpdateReport", parameters);
        // }

        //// Delete a report
        // public void DeleteReport(int reportID)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@ReportID", reportID)
        //    };

        // dbConnection.ExecuteNonQuery("DeleteReport", parameters);
        // }

        //// Add a feed post
        // public void AddFeedPost(string title, string category, string content)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Title", title),
        //        new SqlParameter("@Category", category),
        //        new SqlParameter("@Content", content)
        //    };

        // dbConnection.ExecuteNonQuery("AddFeedPost", parameters);
        // }

        //// Update a feed post
        // public void UpdateFeedPost(int postID, string title, string category, string content)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@PostID", postID),
        //        new SqlParameter("@Title", title),
        //        new SqlParameter("@Category", category),
        //        new SqlParameter("@Content", content)
        //    };

        // dbConnection.ExecuteNonQuery("UpdateFeedPost", parameters);
        // }

        //// Delete a feed post
        // public void DeleteFeedPost(int postID)
        // {
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@PostID", postID)
        //    };

        // dbConnection.ExecuteNonQuery("DeleteFeedPost", parameters);
        // }
        public void AddUserToChat(int userID, int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
            };
            this.dbConnection.ExecuteNonQuery("AddUserToChat", parameters);
        }

        public void RemoveUserFromChat(int userID, int chatID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@ChatID", chatID),
            };
            this.dbConnection.ExecuteNonQuery("RemoveUserFromChat", parameters);
        }
    }
}