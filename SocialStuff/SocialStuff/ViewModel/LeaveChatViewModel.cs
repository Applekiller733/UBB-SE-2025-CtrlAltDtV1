


namespace SocialStuff.ViewModel
{
    using System.ComponentModel;
    using System.Windows.Input;
    using SocialStuff.Services.Interfaces;

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
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LeaveChatViewModel(IUserService userService, IChatService chatService, ChatListViewModel chatMessagesViewModel, int ChatID)
        {
            this.LeaveChatCommand = new RelayCommand(this.LeaveChat);

            this.ChatID = ChatID;
            this.userService = userService;
            this.chatService = chatService;
            this.lastViewModel = chatMessagesViewModel;
        }

        public void LeaveChat()
        {
            this.userService.LeaveChat(this.userService.GetCurrentUser(), this.ChatID);

            this.lastViewModel.LoadChats();
        }
    }
}
