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


namespace SocialStuff.ViewModel
{
    public class ChatMessagesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Message> ChatMessages { get; set; }
        public MessageService MessageService;
        public ChatService ChatService;
        public UserService UserService;
        public int CurrentChatID = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public ChatMessagesViewModel(MessageService msgService, ChatService chtService, UserService usrService)
        {
            ChatMessages = new ObservableCollection<Message>();
            MessageService = msgService;
            ChatService = chtService;
            UserService = usrService;
            this.LoadMessagesForChat();
        }

        private void LoadMessagesForChat()
        {
            ChatMessages.Clear();
            var messages = this.ChatService.getChatHistory(this.CurrentChatID);

            foreach (var message in messages)
            {
                if (message is TextMessage textMessage)
                {
                    ChatMessages.Add(new TextMessage
                        (textMessage.MessageID, textMessage.SenderID, textMessage.ChatID, textMessage.Timestamp, textMessage.Content, textMessage.UsersReport));
                }

                if (message is ImageMessage imageMessage)
                {
                    ChatMessages.Add(new ImageMessage
                        (imageMessage.MessageID, imageMessage.SenderID, imageMessage.ChatID, imageMessage.Timestamp, imageMessage.ImageURL, imageMessage.UsersReport));
                }

                if (message is TransferMessage transferMessage)
                {
                    ChatMessages.Add(new TransferMessage
                        (transferMessage.MessageID, transferMessage.SenderID, transferMessage.ChatID, transferMessage.Status, transferMessage.Amount, transferMessage.Description, transferMessage.Currency));
                }

                if (message is RequestMessage requestMessage)
                {
                    ChatMessages.Add(new RequestMessage
                        (requestMessage.MessageID, requestMessage.SenderID, requestMessage.ChatID, requestMessage.Status, requestMessage.Amount, requestMessage.Description, requestMessage.Currency));
                }
            }
        }


    }

}
