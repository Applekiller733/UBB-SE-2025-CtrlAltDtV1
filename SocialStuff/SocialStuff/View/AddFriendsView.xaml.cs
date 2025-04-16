// <copyright file="AddFriendsView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    public sealed partial class AddFriendsView : Page
    {
        private IUserService userService;
        private FriendsListViewModel friendsListViewModel;
        private AddFriendsViewModel addFriendsViewModel;

        public AddFriendsView(FriendsListViewModel friendsListViewModel, IUserService userService)
        {
            this.InitializeComponent();

            this.friendsListViewModel = friendsListViewModel;
            this.userService = userService;
            this.addFriendsViewModel = new AddFriendsViewModel(friendsListViewModel, userService);

            this.DataContext = this.addFriendsViewModel;
        }
    }
}
