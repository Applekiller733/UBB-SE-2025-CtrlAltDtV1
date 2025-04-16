using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialStuff.ViewModel
{
    public class LeaveChatViewModel : INotifyPropertyChanged
    {
        public ICommand LeaveChatCommand { get; set; }

        private IUserService userService;
        private IChatService chatService;
        private ChatListViewModel lastViewModel;
        private int ChatID;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LeaveChatViewModel(IUserService userService, IChatService chatService, ChatListViewModel chatMessagesViewModel, int ChatID)
        {
            LeaveChatCommand = new RelayCommand(LeaveChat);

            this.ChatID = ChatID;
            this.userService = userService;
            this.chatService = chatService;
            this.lastViewModel = chatMessagesViewModel;
        }

        public void LeaveChat()
        {
            this.userService.LeaveChat(this.userService.GetCurrentUser(), ChatID);

            this.lastViewModel.LoadChats();
        }

    }
}
