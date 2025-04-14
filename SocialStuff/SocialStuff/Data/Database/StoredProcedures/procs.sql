USE BankingDB
go

CREATE OR ALTER PROCEDURE AddChat
@ChatName NVARCHAR(255),
@ChatID INT OUTPUT  -- OUTPUT parameter to return the ID
AS
BEGIN
INSERT INTO Chats (ChatName)
VALUES (@ChatName);

SET @ChatID = SCOPE_IDENTITY();  -- Get the last inserted ChatID

END;

GO

create or alter procedure UpdateChat(
@ChatID INT,
@ChatName VARCHAR(255)
)
AS
BEGIN
UPDATE CHATS
SET ChatName = @ChatName
WHERE ChatID = @ChatID;
END;
go

CREATE OR ALTER PROCEDURE DeleteChat(
@ChatID INT
)
AS
BEGIN
DELETE FROM CHATS WHERE ChatID = @ChatID;
END;
go

CREATE OR ALTER PROCEDURE AddFriend
@UserID int,
@FriendID int
AS
BEGIN
INSERT INTO FRIENDS (UserID, FriendID)
values (@UserID, @FriendID)
end
GO

CREATE OR ALTER PROCEDURE DeleteFriend(
@UserID INT,
@FriendID INT
)
AS
BEGIN
DELETE FROM FRIENDS WHERE (UserID = @UserID AND FriendID = @FriendID) OR (UserID = @FriendID AND FriendID = @UserID);
END;
go

CREATE or alter PROCEDURE AddUser(
@Username VARCHAR(255),
@PhoneNumber VARCHAR(20)
)
AS
BEGIN
INSERT INTO USERS (Username, PhoneNumber)
VALUES (@Username, @PhoneNumber);
END;
go

go
CREATE or alter PROCEDURE UpdateUser(
@UserID INT,
@Username VARCHAR(255),
@PhoneNumber VARCHAR(20)
)
AS
BEGIN
UPDATE USERS
SET Username = @Username, PhoneNumber = @PhoneNumber
WHERE UserID = @UserID;
END;
go

CREATE or alter PROCEDURE DeleteUser(
@UserID INT
)
AS
BEGIN
DELETE FROM USERS WHERE UserID = @UserID;
END;
go

CREATE OR ALTER PROCEDURE AddChatParticipant(
@ChatID INT,
@UserID INT
)
AS
BEGIN
INSERT INTO CHAT_PARTICIPANTS (ChatID, UserID)
VALUES (@ChatID, @UserID);
END;
go

CREATE OR ALTER PROCEDURE RemoveChatParticipant(
@ChatID INT,
@UserID INT
)
AS
BEGIN
DELETE FROM CHAT_PARTICIPANTS WHERE ChatID = @ChatID AND UserID = @UserID;
END;
go

CREATE OR ALTER PROCEDURE AddMessageType(
@TypeName VARCHAR(255)
)
AS
BEGIN
INSERT INTO MESSAGE_TYPE (TypeName)
VALUES (@TypeName);
END;
go

CREATE OR ALTER PROCEDURE DeleteMessageType(
@TypeID INT
)
AS
BEGIN
DELETE FROM MESSAGE_TYPE WHERE TypeID = @TypeID;
END;
go

CREATE OR ALTER PROCEDURE AddMessage(
@TypeID INT,
@UserID INT,
@ChatID INT,
@Content NVARCHAR(MAX),  -- Changed from VARCHAR to NVARCHAR
@Status NVARCHAR(255) = NULL,  -- Changed from VARCHAR to NVARCHAR
@Amount DECIMAL(10,2) = NULL,
@Currency NVARCHAR(10) = NULL  -- Changed from VARCHAR to NVARCHAR
)
AS
BEGIN
INSERT INTO MESSAGES (TypeID, UserID, ChatID, Content, Status, Amount, Currency)
VALUES (@TypeID, @UserID, @ChatID, @Content, @Status, @Amount, @Currency);
END;
GO

CREATE OR ALTER PROCEDURE UpdateMessage(
@MessageID INT,
@TypeID INT,
@Content VARCHAR(255),
@Status VARCHAR(255) = NULL,
@Amount DECIMAL(10,2) = NULL,
@Currency VARCHAR(10) = NULL
)
AS
BEGIN
UPDATE MESSAGES
SET TypeID = @TypeID, Content = @Content, Status = @Status, Amount = @Amount, Currency = @Currency
WHERE MessageID = @MessageID;
END;
go

CREATE OR ALTER PROCEDURE DeleteMessage(
@MessageID INT
)
AS
BEGIN
DELETE FROM MESSAGES WHERE MessageID = @MessageID;
END;
GO

CREATE OR ALTER PROCEDURE AddReport(
@MessageID INT,
@Reason VARCHAR(255),
@Description VARCHAR(255) = NULL,
@Status VARCHAR(50)
)
AS
BEGIN
INSERT INTO REPORTS (MessageID, Reason, Description, Status)
VALUES (@MessageID, @Reason, @Description, @Status);
END;
GO

CREATE OR ALTER PROCEDURE UpdateReport(
@ReportID INT,
@Reason VARCHAR(255),
@Description VARCHAR(255) = NULL,
@Status VARCHAR(50)
)
AS
BEGIN
UPDATE REPORTS
SET Reason = @Reason, Description = @Description, Status = @Status
WHERE ReportID = @ReportID;
END;
GO

CREATE OR ALTER PROCEDURE DeleteReport(
@ReportID INT
)
AS
BEGIN
DELETE FROM REPORTS WHERE ReportID = @ReportID;
END;
GO

CREATE OR ALTER PROCEDURE AddNotification(
@Content VARCHAR(255),
@UserID INT
)
AS
BEGIN
INSERT INTO NOTIFICATIONS (Content, UserID)
VALUES (@Content, @UserID);
END;
GO

CREATE OR ALTER PROCEDURE DeleteNotification
@NotifID INT
AS
BEGIN
DELETE FROM Notifications WHERE NotifID = @NotifID;
END;
GO

CREATE OR ALTER PROCEDURE DeleteAllNotifications
@UserID INT
AS
BEGIN
DELETE FROM Notifications WHERE UserID = @UserID;
END;
GO

CREATE OR ALTER PROCEDURE AddFeedPost(
@Title VARCHAR(255),
@Category VARCHAR(255),
@Content VARCHAR(255)
)
AS
BEGIN
INSERT INTO FEED_POSTS (Title, Category, Content)
VALUES (@Title, @Category, @Content);
END;
GO

CREATE OR ALTER PROCEDURE UpdateFeedPost(
@PostID INT,
@Title VARCHAR(255),
@Category VARCHAR(255),
@Content VARCHAR(255)
)
AS
BEGIN
UPDATE FEED_POSTS
SET Title = @Title, Category = @Category, Content = @Content
WHERE PostID = @PostID;
END;
GO

CREATE OR ALTER PROCEDURE DeleteFeedPost(
@PostID INT
)
AS
BEGIN
DELETE FROM FEED_POSTS WHERE PostID = @PostID;
END;
GO

-- Procedure to add a user to a chat
CREATE OR ALTER PROCEDURE AddUserToChat(
@UserID INT,
@ChatID INT
)
AS
BEGIN
INSERT INTO CHAT_PARTICIPANTS (UserID, ChatID)
VALUES (@UserID, @ChatID);
END;
GO

-- Procedure to remove a user from a chat
CREATE OR ALTER PROCEDURE RemoveUserFromChat(
@UserID INT,
@ChatID INT
)
AS
BEGIN
DELETE FROM CHAT_PARTICIPANTS
WHERE UserID = @UserID AND ChatID = @ChatID;
END;
GO
