// <copyright file="ReportViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using SocialStuff.Model;
    using SocialStuff.Services.Interfaces;

    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly int reportedUserId;
        private readonly int messageId;

        private string selectedCategory;

        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (this.selectedCategory != value)
                {
                    this.selectedCategory = value;
                    this.OnPropertyChanged(nameof(this.SelectedCategory));
                    this.OnPropertyChanged(nameof(this.IsOtherCategorySelected));
                }
            }
        }

        public bool IsOtherCategorySelected => this.SelectedCategory == "Other";

        private string otherReason;

        public string OtherReason
        {
            get => otherReason;
            set
            {
                if (otherReason != value)
                {
                    otherReason = value;
                    OnPropertyChanged(nameof(OtherReason));
                }
            }
        }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReportViewModel(IUserService userService, IReportService reportService, int reportedUserId, int messageId)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.reportedUserId = reportedUserId;
            this.messageId = messageId;

            this.SubmitCommand = new RelayCommand(this.SubmitReport);
            this.CancelCommand = new RelayCommand(this.CancelReport);
        }

        private async void SubmitReport()
        {
            string reason = this.SelectedCategory == "Other" ? this.OtherReason : this.SelectedCategory;

            if (string.IsNullOrEmpty(reason))
            {
                // Trigger an event or callback to show the dialog in the view
                this.OnShowErrorDialog("Please provide a reason for reporting.");
                return;
            }

            // Create a new report
            Report report = new Report(this.messageId, this.reportedUserId, "Pending", reason, string.Empty);
            this.reportService.AddReport(report);
            this.reportService.LogReportedMessages(new List<Report> { report });

            // Increase the reported count for the user
            User reportedUser = this.userService.GetUserById(reportedUserId);
            string reportMessage = reportedUser.IncreaseReportCount();

            this.userService.MarkUserAsDangerousAndGiveTimeout(reportedUser);

            // this.userService.setUserTimeout(true);  //MADE IT NONSTATIC HERE TOO!!!

            // Trigger an event or callback to show the success dialog in the view
            this.OnShowSuccessDialog("Report submitted successfully.");
        }

        private void CancelReport()
        {
            // Trigger an event or callback to close the view
            this.OnCloseView();
        }

        // Define events or callbacks for showing dialogs and closing the view
        public event Action<string> ShowErrorDialog;

        public event Action<string> ShowSuccessDialog;

        public event Action CloseView;

        protected virtual void OnShowErrorDialog(string message)
        {
            this.ShowErrorDialog?.Invoke(message);
        }

        protected virtual void OnShowSuccessDialog(string message)
        {
            this.ShowSuccessDialog?.Invoke(message);
        }

        protected virtual void OnCloseView()
        {
            this.CloseView?.Invoke();
        }
    }
}