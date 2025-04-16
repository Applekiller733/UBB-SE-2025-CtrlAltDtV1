using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
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

        public IUserService userService { get; set; }

        private FriendsListViewModel friendsListViewModel;

        public ICommand AddFriendCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public AddFriendsViewModel(FriendsListViewModel friendsListViewModel, IUserService userService)
        {
            this.userService = userService;
            this.friendsListViewModel = friendsListViewModel;
            this.allUsers = userService.GetNonFriendsUsers(userService.GetCurrentUser());
            this.UsersList = new ObservableCollection<User>();
            this.AddFriendCommand = new RelayCommand<object>(AddFriend);

            this.LoadUsers();
        }

        private void AddFriend(object user)
        {
            var friend = user as User;

            this.userService.AddFriend(this.userService.GetCurrentUser(), friend.GetUserId());
            this.friendsListViewModel.LoadFriends();
            this.LoadUsers();
        }

        private void LoadUsers()
        {
            this.allUsers = this.userService.GetNonFriendsUsers(this.userService.GetCurrentUser());
            this.FilterUsers();
        }

        private void FilterUsers()
        {
            this.UsersList.Clear();

            foreach (var friend in this.allUsers.Where(f =>
                         string.IsNullOrEmpty(this.SearchQuery) ||
                         f.Username.Contains(this.SearchQuery, StringComparison.OrdinalIgnoreCase)))
            {
                this.UsersList.Add(friend);
            }
        }
    }
}
