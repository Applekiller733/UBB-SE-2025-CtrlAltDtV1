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
        public int CurrentChatID = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public ChatMessagesViewModel()
        {
            ChatMessages = new ObservableCollection<Message>();
            Repository repo = new Repository();
            MessageService = new MessageService(repo);
            ChatService = new ChatService(repo);
            this.LoadMessagesForChat();
        }

        private void LoadMessagesForChat()
        {
            ChatMessages.Clear();
            var messages = this.ChatService.getChatHistory(this.CurrentChatID);

            foreach (var message in messages)
            {
                ChatMessages.Add(message);
            }
        }


    }

}
