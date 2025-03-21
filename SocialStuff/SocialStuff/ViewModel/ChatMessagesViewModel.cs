using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SocialStuff.Model.MessageClasses;
using MvvmHelpers;
using SocialStuff.Services;
using Windows.ApplicationModel.Chat;
using SocialStuff.Data;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using SocialStuff.View;


namespace SocialStuff.ViewModel
{
    public class ChatMessagesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Message> ChatMessages { get; set; }
        public MessageService messageService;
        public ChatService chatService;
        public UserService userService;
        private MessageTemplateSelector templateSelector;
        public int CurrentChatID = 1;
        public int CurrentUserID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChatMessagesViewModel(MessageService msgService, ChatService chtService, UserService usrService)
        {
            ChatMessages = new ObservableCollection<Message>();
            messageService = msgService;
            chatService = chtService;
            userService = usrService;
            this.CurrentUserID = userService.GetCurrentUser();

            templateSelector = new MessageTemplateSelector()
            {
                TextMessageTemplateLeft = (DataTemplate)App.Current.Resources["TextMessageTemplateLeft"],
                TextMessageTemplateRight = (DataTemplate)App.Current.Resources["TextMessageTemplateRight"],
                ImageMessageTemplateLeft = (DataTemplate)App.Current.Resources["ImageMessageTemplateLeft"],
                ImageMessageTemplateRight = (DataTemplate)App.Current.Resources["ImageMessageTemplateRight"],
                TransferMessageTemplateLeft = (DataTemplate)App.Current.Resources["TransferMessageTemplateLeft"],
                TransferMessageTemplateRight = (DataTemplate)App.Current.Resources["TransferMessageTemplateRight"],
                RequestMessageTemplateLeft = (DataTemplate)App.Current.Resources["RequestMessageTemplateLeft"],
                RequestMessageTemplateRight = (DataTemplate)App.Current.Resources["RequestMessageTemplateRight"]
            };

            this.LoadMessagesForChat();
        }

        private void LoadMessagesForChat()
        {
            ChatMessages.Clear();
            var messages = this.chatService.getChatHistory(this.CurrentChatID);

            foreach (var message in messages)
            {
                if (message is TextMessage textMessage)
                {
                    ChatMessages.Add(new TextMessage 
                        (textMessage.getMessageID(), textMessage.getSenderID(), textMessage.getChatID(), textMessage.getTimestamp(), textMessage.getContent(), textMessage.getUsersReport()));
                }

                if (message is ImageMessage imageMessage)
                {
                    ChatMessages.Add(new ImageMessage
                        (imageMessage.getMessageID(), imageMessage.getSenderID(), imageMessage.getChatID(), imageMessage.getTimestamp(), imageMessage.getImageURL(), imageMessage.getUsersReport()));
                }

                if (message is TransferMessage transferMessage)
                {
                    ChatMessages.Add(new TransferMessage
                        (transferMessage.getMessageID(), transferMessage.getSenderID(), transferMessage.getChatID(), transferMessage.getStatus(), transferMessage.getAmount(), transferMessage.getDescription(), transferMessage.getCurrency()));
                }

                if (message is RequestMessage requestMessage)
                {
                    ChatMessages.Add(new RequestMessage
                        (requestMessage.getMessageID(), requestMessage.getSenderID(), requestMessage.getChatID(), requestMessage.getStatus(), requestMessage.getAmount(), requestMessage.getDescription(), requestMessage.getCurrency()));
                }
            }
        }


    }

}
