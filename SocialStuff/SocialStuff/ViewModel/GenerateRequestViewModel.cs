using SocialStuff.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SocialStuff.ViewModel
{
    public class GenerateRequestViewModel : INotifyPropertyChanged
    {
        private float amount;
        private string currency;
        private string description;
        private string selectedTransferType;
        private readonly ChatService chatService;

        public GenerateRequestViewModel(ChatService chatService)
        {
            this.chatService = chatService;
            SendMessageCommand = new RelayCommand(SendMessage);
        }

        public float Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public string Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTransferType
        {
            get => selectedTransferType;
            set
            {
                selectedTransferType = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SendMessage()
        {
            int chatID = chatService.getCurrentChatID();
            int userID = chatService.GetCurrentUserID();

            switch (SelectedTransferType)
            {
                case "Transfer Money":
                    chatService.sendMoneyViaChat(Amount, Currency, Description, chatID);
                    break;
                case "Request Money":
                    chatService.requestMoneyViaChat(Amount, Currency, chatID, Description);
                    break;
                case "Split Bill":
                    // Implement split bill logic here
                    break;
                default:
                    throw new InvalidOperationException("Invalid transfer type selected.");
            }
        }
    }
}
