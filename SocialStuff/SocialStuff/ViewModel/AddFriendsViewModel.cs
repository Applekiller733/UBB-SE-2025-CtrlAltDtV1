using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocialStuff.ViewModel
{
    public class AddFriendsViewModel : INotifyPropertyChanged
    {
        public List<User> allUsers { get; set; }
        public ObservableCollection<User> UsersList { get; set; }
        public UserService userService { get; set; }
        private FriendsListViewModel friendsListViewModel;
        public ICommand AddFriendCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged(nameof(SearchQuery));
                    FilterUsers();
                }
            }
        }
        public AddFriendsViewModel(FriendsListViewModel friendsListViewModel, UserService userService)
        {
            this.userService = userService;
            this.friendsListViewModel = friendsListViewModel;
            this.allUsers = userService.GetNonFriendsUsers(userService.GetCurrentUser());
            UsersList = new ObservableCollection<User>();
            AddFriendCommand = new RelayCommand<object>(AddFriend);

            LoadUsers();
        }

        private void AddFriend(object user)
        {
            var friend = user as User;

            this.userService.AddFriend(this.userService.GetCurrentUser(), friend.GetUserId());
            friendsListViewModel.LoadFriends();
            LoadUsers();
        }

        private void LoadUsers()
        {
            this.allUsers = userService.GetNonFriendsUsers(userService.GetCurrentUser());
            FilterUsers();
        }

        private void FilterUsers()
        {
            UsersList.Clear();

            foreach (var friend in allUsers.Where(f =>
                         string.IsNullOrEmpty(SearchQuery) ||
                         f.Username.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                UsersList.Add(friend);
            }
        }
    }
}
