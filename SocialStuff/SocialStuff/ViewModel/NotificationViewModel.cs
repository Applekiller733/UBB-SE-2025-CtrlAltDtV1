// <copyright file="NotificationViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class NotificationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ClearNotificationCommand { get; }

        public ICommand ClearAllNotificationsCommand { get; }

        private ObservableCollection<Notification> notifications;

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return notifications;
            }

            set
            {
                this.notifications = value;
                this.OnPropertyChanged(nameof(this.Notifications));
            }
        }

        private readonly int currentUserID;

        private readonly INotificationService notificationService;

        public NotificationViewModel()
        {
            // constructor
        }

        public NotificationViewModel(INotificationService service, int userID)
        {
            this.notificationService = service;
            this.currentUserID = userID;
            this.Notifications = new ObservableCollection<Notification>();
            this.ClearNotificationCommand = new RelayCommand<int>(this.ClearNotification);
            this.ClearAllNotificationsCommand = new RelayCommand(this.ClearAllNotifications);
            this.LoadNotifications();
        }

        public void LoadNotifications()
        {
            var notificationsList = this.notificationService.GetNotifications(this.currentUserID);
            this.Notifications = new ObservableCollection<Notification>(notificationsList);
        }

        public void ClearNotification(int notificationID)
        {
            this.notificationService.ClearNotification(notificationID);
            this.LoadNotifications();
        }

        public void ClearAllNotifications()
        {
            this.notificationService.ClearAllNotifications(this.currentUserID);
            this.LoadNotifications();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}