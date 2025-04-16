// <copyright file="NotificationView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Data;
    using SocialStuff.Services.Implementations;
    using SocialStuff.ViewModel;

    public sealed partial class NotificationView : Page
    {
        private NotificationViewModel viewModel;

        public NotificationView()
        {
            this.InitializeComponent();
            var repo = new Repository();
            var notificationService = new NotificationService(repo);
            var userService = new UserService(repo, notificationService);
            int currentUserID = repo.GetLoggedInUserID();
            this.DataContext = new NotificationViewModel(notificationService, currentUserID);
        }
    }
}