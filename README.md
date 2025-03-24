# UBB-SE-2025-CtrlAltDefeat
**Team Name:** CtrlAltDefeat

**Team Members:** Alexe Răzvan (Team Lead), Bălă Bogdan, Balog David, Albu Maria, Beudean Carmen, Alistar Alexandra, Bardan Irina

**Functionalities summary:**

## Chat Functionality

- chat to one user or with a group of users

A user can participate in a conversation either one-on-one or in a group by exchanging messages within a chat.  Messages can be a text with max 256 characters including emojis. A message can alternatively be a picture. All the messages will be saved in the chat, they will be available for both users at any point in time. The messages will be verified in order to check their integrity after sending and they will persist in the chat. The messages also need to be sent in a timely manner (no more than 2 seconds, on a broadband connection of minimum 25Mbps).

## Chat List Functionality

A user can view all of the chats that they are a part of, sorted descending by the most recent message of each chat. This makes up the main screen of the application and includes chats that the user has created or chats that the user was added to. The user can select any of the available conversations, which opens the respective chat window.

## Create a Chat Functionality

A user can create a chat using a ‘+’ button in the chat list window. This prompts the opening of a smaller window that contains a list of all of the users in your friends list, where the user can give a name to the chat using an input field, filter the friends by the username in a search bar, and can select which users to add in the chat. Upon confirmation by pressing the ‘Create Chat’ button, a new chat appears in the list of chats window for all of the users added in the chat.

## Transfer Functionality

- transfer to one in a single chat or transfer to all in a group

A user can initiate a transfer through a chat by pressing the ‘Transfer’ button in a conversation. When initiating a transfer in a one-on-one conversation, a new window appears. The window prompts the user to input the sum they want to transfer to that person, which currency to use in the transfer, and a short description consisting of a maximum of 100 text-only characters. If the transfer has been successful, a new special message will appear in the chat, appearing as though it had been sent by the user, containing the sum, the currency, and the description of the transfer. Alternatively, if the user wants to initiate a transfer within a group chat, they can send an amount of money to all the participants in the group. After pressing the ‘Transfer’ button, the same transfer window from before appears, and the sum selected will be sent to each person in the group. Again, a new special message will be automatically sent to the chat by the user, showcasing the sum, currency and the description.

## Request Functionality

- request from on in a single chat or from all in a group chat

A user can initiate a request directly through the chat by pressing a ‘Request’ button in any chat. In a one-on-one conversation, this opens a new window where the user specifies the requested amount, the currency, and a description of a maximum of 100 text-only characters. Upon confirmation, a special request message is sent in the chat, displaying the amount, currency, and description. If the other user accepts the request by clicking the ‘Accept Request’ button present in the message, a transfer begins with that sum. If the transfer is successful, a transfer message is sent by the other user in the chat with the amount and currency. In a group chat, the user initiates a request by pressing the ‘Request’ button, which opens the same window used for one-on-one requests. After specifying the amount, currency, and the description of a maximum of 100 text-only characters, the request is posted as a request message in the chat. Any member of the group can choose to fulfill the request by clicking the ‘Accept Request’ button, initiating a transaction for the specified amount to the requesting user. If the transaction is successful, each member automatically sends a transfer message with the requested amount and currency to the chat. 

## Split the Bill Functionality

In a group chat or a one-on-one conversation, a user can prompt a split the bill operation using the ‘Split the Bill’ button. This opens a new window where the user needs to input a sum, a currency and a description of the bill to be split of a maximum of 100 text-only characters. Then, a request message is sent from the user to the chat, requesting the amount of the bill divided by the amount of users in the conversation, with the correct currency and description. Any user in the chat can respond to the request message by pressing the ‘Accept Request’ button included in the message, which initiates a transfer with that amount of money and currency. If the transfer is successful, a transfer message will be sent by that user in the chat, specifying the amount of money and the currency.

## Notifications Functionality

A user has a notification button which opens a notifications list window, where he can see all the notifications sorted descending by the date and time. Notifications include: chat messages, friend requests and group invites. Chat notifications include the name of the chat and the sender of the message, friend requests include the name of the user along with the message ‘added you as friend’ and group invites include the name of the group along with the message ‘you were added in a new group’. Additionally, the user has the option to clear all notifications by using an ‘x’ button, then the inbox will be emptied.

## Friends List Functionality

A user can view a list of all of their friends, sorted ascending by username by pressing a button inside the main window (the chat list window). Additionally, the user can search for a friend using the search bar from the top of the window, searching by phone number or by username. A friend is a person that has been added through the Add Friend functionality, which is accessed through a button available from the friends list. 

## Remove Friend Functionality

After entering the friends list window, a user can remove a friend by pressing a ‘-’ button that is available on each of the users in the friends list. This does not remove any of the user’s chats.

## Add friend Functionality

A user can add a new friend by using an ‘Add friend’ button in the friends window. This prompts the opening of a new window that contains a search bar at the top of the window, along with a list of all the users from the database. Every user from the list has a displayed username and phone number, along with a button ‘+’ that allows the user to add the selected person as a new friend. Adding a friend is a one-way process where a user adds a person to its list of known or ‘trusted’ people by selecting one of the displayed users or searching for a specific user by phone number or by username. After adding the user as a friend, the new friend will be displayed in the friends list. This process does not need any confirmation from the person that is being added by the user.

## Feed Functionality

A user can access a window named ‘Feed’ by clicking the ‘Feed’ button from the main panel of the application. The Feed contains all the users from the database, where everyone can see any community announcements, which include stocks or gambling information. No user is allowed to send messages in the feed, only to view the posts.

## Report Functionality

A user can send a report for a text/image message through a button. That button is accessed by pressing the 3 points button next to the message . When selecting the report button a pop-up window will appear asking the user for a reason by selecting a category from the drop-down list(Categories will be : Stinky, Bad Language, Ugly Emoji, Scamming etc.). If the selected category is ‘Other’ a new text box will appear. The reported count for that user will be incremented.

## Delete Message Functionality

A user can delete a message sent by him in a specific chat by pressing the 3 point (…) buttons next to the message. After pressing that button, this will prompt the opening of a small window which includes 2 options: report message and delete message. When the user presses on the ‘delete message’ option, a confirmation for deletion prompt will be opened, which allows the user to delete the message or to cancel the operation. After deleting, the message will be permanently deleted from the database and will no longer be visible for any user in the selected chat. The delete operation cannot be undone.

## Leave Chat Functionality

A user can leave a chat by pressing a button from the respective chat window named ‘Leave chat’. After clicking the button, a new prompt confirmation will be opened, which allows the user to leave the chat or to cancel the operation. After leaving a chat, the chat will be deleted from the user’s chat list window, but the chat will persist in the database with the remaining members. If there are no members left, the chat will be deleted from the database.
