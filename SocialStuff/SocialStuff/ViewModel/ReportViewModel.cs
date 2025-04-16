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

    /// <summary>
    /// ViewModel for handling report functionality.
    /// </summary>
    public class ReportViewModel : INotifyPropertyChanged
    {
        public event Action<string> ShowSuccessDialog = message => { };
        public event Action<string> ShowErrorDialog = message => { };
        public event Action CloseView = () => { };

        private readonly IUserService userService;
        private readonly IReportService reportService;
        private readonly int reportedUserId;
        private readonly int messageId;

        private string selectedCategory = string.Empty;
        private string otherReason = string.Empty;

        /// <summary>
        /// Gets or sets the selected category for the report.
        /// </summary>
        public string SelectedCategory
        {
            get => this.selectedCategory;
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

        /// <summary>
        /// Gets a value indicating whether the selected category is "Other".
        /// </summary>
        public bool IsOtherCategorySelected => this.SelectedCategory == "Other";

        /// <summary>
        /// Gets or sets the reason for reporting when the selected category is "Other".
        /// </summary>
        public string OtherReason
        {
            get => this.otherReason;
            set
            {
                if (this.otherReason != value)
                {
                    this.otherReason = value;
                    this.OnPropertyChanged(nameof(this.OtherReason));
                }
            }
        }

        /// <summary>
        /// Gets the command to submit the report.
        /// </summary>
        public ICommand SubmitCommand { get; }

        /// <summary>
        /// Gets the command to cancel the report.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportViewModel"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="reportedUserId">The ID of the reported user.</param>
        /// <param name="messageId">The ID of the message being reported.</param>
        public ReportViewModel(IUserService userService, IReportService reportService, int reportedUserId, int messageId)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.reportedUserId = reportedUserId;
            this.messageId = messageId;

            this.SubmitCommand = new RelayCommand(this.SubmitReport);
            this.CancelCommand = new RelayCommand(this.CancelReport);
        }

        /// <summary>
        /// Submits the report.
        /// </summary>
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
            string reportMessage = reportedUser!.IncreaseReportCount();

            this.userService.MarkUserAsDangerousAndGiveTimeout(reportedUser);

            // Trigger an event or callback to show the success dialog in the view
            this.OnShowSuccessDialog("Report submitted successfully.");
        }

        /// <summary>
        /// Cancels the report.
        /// </summary>
        private void CancelReport()
        {
            // Trigger an event or callback to close the view
            this.OnCloseView();
        }

        /// <summary>
        /// Raises the <see cref="ShowErrorDialog"/> event.
        /// </summary>
        /// <param name="message">The error message to show.</param>
        protected virtual void OnShowErrorDialog(string message)
        {
            this.ShowErrorDialog?.Invoke(message);
        }

        /// <summary>
        /// Raises the <see cref="ShowSuccessDialog"/> event.
        /// </summary>
        /// <param name="message">The success message to show.</param>
        protected virtual void OnShowSuccessDialog(string message)
        {
            this.ShowSuccessDialog?.Invoke(message);
        }

        /// <summary>
        /// Raises the <see cref="CloseView"/> event.
        /// </summary>
        protected virtual void OnCloseView()
        {
            this.CloseView?.Invoke();
        }
    }
}