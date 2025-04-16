using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MvvmHelpers;
using SocialStuff.Data;
using SocialStuff.Model.MessageClasses;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using SocialStuff.View;
using SocialStuff.Views;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SocialStuff.ViewModel
{
    public class ChatMessagesViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        public ObservableCollection<Message> ChatMessages { get; set; }
        public ListView ChatListView { get; set; }
        public IMessageService messageService;
        public IChatService chatService;
        public IUserService userService;
        public IReportService reportService;
        private MessageTemplateSelector templateSelector;
        public int CurrentChatID { get; set; }
        public int CurrentUserID { get; set; }
        public string CurrentChatName { get; set; }

        // For message polling
        private Timer _messagePollingTimer;
        private DateTime _lastMessageTimestamp = DateTime.MinValue;
        private const int POLLING_INTERVAL = 2000; // 2 seconds

        public string CurrentChatParticipantsString => string.Join(", ", CurrentChatParticipants ?? new List<string>());
        private List<string> currentChatParticipants;
        public List<string> CurrentChatParticipants
        {
            get => currentChatParticipants;
            set
            {
                if (currentChatParticipants != value)
                {
                    currentChatParticipants = value;
                    OnPropertyChanged(nameof(CurrentChatParticipants));
                    OnPropertyChanged(nameof(CurrentChatParticipantsString));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string messageContent;
        public string MessageContent
        {
            get => messageContent;
            set
            {
                if (messageContent != value)
                {
                    messageContent = value;
                    OnPropertyChanged(nameof(MessageContent));
                    OnPropertyChanged(nameof(RemainingCharacterCount));
                }
            }
        }
        public int RemainingCharacterCount => 256 - (MessageContent?.Length ?? 0);

        public ICommand SendMessageCommand { get; }
        private void SendMessage()
        {
            string convertedContent = EmoticonConverter.ConvertEmoticonsToEmojis(MessageContent);
            this.messageService.SendMessage(CurrentUserID, CurrentChatID, convertedContent);
            this.CheckForNewMessages();
            MessageContent = "";
        }

        public ICommand SendImageCommand { get; }
        private async void SendImage()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var hwnd = WindowNative.GetWindowHandle(_window);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string imageUrl = await ImgurImageUploader.UploadImageAndGetUrl(file);
                this.messageService.SendImage(CurrentUserID, CurrentChatID, imageUrl);
                this.CheckForNewMessages();
            }
        }

        public void ScrollToBottom()
        {
            if (ChatListView != null)
            {
                ChatListView.DispatcherQueue.TryEnqueue(() =>
                {
                    var scrollViewer = FindVisualChild<ScrollViewer>(ChatListView);
                    scrollViewer?.ChangeView(null, scrollViewer.ScrollableHeight, null);
                });
            }
        }

        public ICommand DeleteMessageCommand { get; set; }
        private void DeleteMessage(Message message)
        {
            this.messageService.DeleteMessage(message);
            this.LoadAllMessagesForChat();
        }

        public ICommand ReportMessageCommand { get; set; }
        private void ReportMessage(Message message)
        {
            // Navigate to ReportView

            ReportView reportView = new ReportView(userService, reportService, message.GetSenderID(), message.GetMessageID());
            reportView.Activate();
        }


        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        public ChatMessagesViewModel(Window window,Frame RightFrame, int currentChatID, IMessageService msgService, IChatService chtService, IUserService usrService, IReportService reportService)
        {
            _window = window;
            ChatMessages = new ObservableCollection<Message>();
            messageService = msgService;
            chatService = chtService;
            userService = usrService;
            this.reportService = reportService;
            this.CurrentChatID = currentChatID;
            this.CurrentUserID = userService.GetCurrentUser();
            this.SendMessageCommand = new RelayCommand(SendMessage);
            this.SendImageCommand = new RelayCommand(SendImage);
            this.CurrentChatName = chatService.GetChatNameByID(CurrentChatID);
            this.CurrentChatParticipants = chatService.GetChatParticipantsStringList(CurrentChatID);
            this.DeleteMessageCommand = new RelayCommand<Message>(DeleteMessage);
            this.ReportMessageCommand = new RelayCommand<Message>(ReportMessage);

            templateSelector = new MessageTemplateSelector()
            {
                TextMessageTemplateLeft = (DataTemplate)App.Current.Resources["TextMessageTemplateLeft"],
                TextMessageTemplateRight = (DataTemplate)App.Current.Resources["TextMessageTemplateRight"],
                ImageMessageTemplateLeft = (DataTemplate)App.Current.Resources["ImageMessageTemplateLeft"],
                ImageMessageTemplateRight = (DataTemplate)App.Current.Resources["ImageMessageTemplateRight"],
                TransferMessageTemplateLeft = (DataTemplate)App.Current.Resources["TransferMessageTemplateLeft"],
                TransferMessageTemplateRight = (DataTemplate)App.Current.Resources["TransferMessageTemplateRight"],
                RequestMessageTemplateLeft = (DataTemplate)App.Current.Resources["RequestMessageTemplateLeft"],
                RequestMessageTemplateRight = (DataTemplate)App.Current.Resources["RequestMessageTemplateRight"],

            };

            // Initial load of messages
            this.LoadAllMessagesForChat();
            this.ScrollToBottom();

            // Start polling for new messages
            StartMessagePolling();
        }

        // Initial load of all messages
        private void LoadAllMessagesForChat()
        {
            ChatMessages.Clear();
            var messages = this.chatService.GetChatHistory(this.CurrentChatID);

            foreach (var message in messages)
            {
                AddMessageToChat(message);
            }

            // Update the last message timestamp
            if (ChatMessages.Any())
            {
                _lastMessageTimestamp = ChatMessages.Max(m => m.GetTimestamp());
            }

            ScrollToBottom();
        }

        // Start polling for new messages
        private void StartMessagePolling()
        {
            // Dispose of existing timer if it exists
            _messagePollingTimer?.Dispose();

            // Create new timer that checks for new messages
            _messagePollingTimer = new Timer(
                _ => ChatListView?.DispatcherQueue.TryEnqueue(() => CheckForNewMessages()),
                null,
                0,
                POLLING_INTERVAL
            );
        }

        // Stop polling for messages
        public void StopMessagePolling()
        {
            this._messagePollingTimer?.Dispose();
            this._messagePollingTimer = null;
        }

        // Check for new messages by comparing timestamps
        private void CheckForNewMessages()
        {
            var messages = this.chatService.GetChatHistory(this.CurrentChatID);
            bool hasNewMessages = false;

            foreach (var message in messages)
            {
                // If the message timestamp is newer than the last message we processed
                if (message.GetTimestamp() > this._lastMessageTimestamp)
                {
                    this.AddMessageToChat(message);
                    hasNewMessages = true;

                    // Update the last message timestamp if this is newer
                    if (message.GetTimestamp() > this._lastMessageTimestamp)
                    {
                        this._lastMessageTimestamp = message.GetTimestamp();
                    }
                }
            }

            // Only scroll if we added new messages
            if (hasNewMessages)
            {
                this.ScrollToBottom();
            }
        }

        // Helper method to add a message to the chat
        private void AddMessageToChat(Message message)
        {
            // Process message based on its type
            if (message is TextMessage textMessage)
            {
                TextMessage newTextMessage = new TextMessage(
                    textMessage.GetMessageID(),
                    textMessage.GetSenderID(),
                    textMessage.GetChatID(),
                    textMessage.GetTimestamp(),
                    textMessage.GetContent(),
                    textMessage.GetUsersReport());
                newTextMessage.SenderUsername = this.userService.GetUserById(textMessage.GetSenderID()).GetUsername();
                this.ChatMessages.Add(newTextMessage);
            }
            else if (message is ImageMessage imageMessage)
            {
                ImageMessage newImageMessage = new ImageMessage(
                    imageMessage.GetMessageID(),
                    imageMessage.GetSenderID(),
                    imageMessage.GetChatID(),
                    imageMessage.GetTimestamp(),
                    imageMessage.GetImageURL(),
                    imageMessage.GetUsersReport());
                newImageMessage.SenderUsername = this.userService.GetUserById(imageMessage.GetSenderID()).GetUsername();
                this.ChatMessages.Add(newImageMessage);
            }
            else if (message is TransferMessage transferMessage)
            {
                TransferMessage newTransferMessage = new TransferMessage(
                    transferMessage.GetMessageID(),
                    transferMessage.GetSenderID(),
                    transferMessage.GetChatID(),
                    transferMessage.GetStatus(),
                    transferMessage.GetAmount(),
                    transferMessage.GetDescription(),
                    transferMessage.GetCurrency());
                newTransferMessage.SenderUsername = this.userService.GetUserById(transferMessage.GetSenderID()).GetUsername();
                this.ChatMessages.Add(newTransferMessage);
            }
            else if (message is RequestMessage requestMessage)
            {
                RequestMessage newRequestMessage = new RequestMessage(
                    requestMessage.GetMessageID(),
                    requestMessage.GetSenderID(),
                    requestMessage.GetChatID(),
                    requestMessage.GetStatus(),
                    requestMessage.GetAmount(),
                    requestMessage.GetDescription(),
                    requestMessage.GetCurrency());
                newRequestMessage.SenderUsername = this.userService.GetUserById(requestMessage.GetSenderID()).GetUsername();
                this.ChatMessages.Add(newRequestMessage);
            }
        }

        public void SetupMessageTracking()
        {
            this.ChatMessages.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add ||
                    e.Action == NotifyCollectionChangedAction.Reset)
                {
                    this.ScrollToBottom();
                }
            };
        }

        // Important: Call this method when the view is being unloaded or when navigating away
        public void Cleanup()
        {
            this.StopMessagePolling();
        }
    }
}