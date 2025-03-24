using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SocialStuff.Model;
using SocialStuff.Services;


namespace SocialStuff.ViewModel
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly UserService userService;
        private readonly ReportService reportService;
        private readonly int reportedUserId;
        private readonly int messageId;

        private string selectedCategory;
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    OnPropertyChanged(nameof(IsOtherCategorySelected));
                }
            }
        }

        public bool IsOtherCategorySelected => SelectedCategory == "Other";

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReportViewModel(UserService userService, ReportService reportService, int reportedUserId, int messageId)
        {
            this.userService = userService;
            this.reportService = reportService;
            this.reportedUserId = reportedUserId;
            this.messageId = messageId;

            SubmitCommand = new RelayCommand(SubmitReport);
            CancelCommand = new RelayCommand(CancelReport);
        }

        private async void SubmitReport()
        {
            string reason = SelectedCategory == "Other" ? OtherReason : SelectedCategory;

            if (string.IsNullOrEmpty(reason))
            {
                // Trigger an event or callback to show the dialog in the view
                OnShowErrorDialog("Please provide a reason for reporting.");
                return;
            }

            // Create a new report
            Report report = new Report(messageId, reportedUserId, "Pending", reason, string.Empty);
            reportService.AddReport(report);
            reportService.LogReportedMessages(new List<Report> { report });

            // Increase the reported count for the user
            User reportedUser = userService.GetUserById(reportedUserId);
            string reportMessage = reportedUser.IncreaseReportCount();

            userService.MarkUserAsDangerousAndGiveTimeout(reportedUser);

            // Trigger an event or callback to show the success dialog in the view
            OnShowSuccessDialog("Report submitted successfully.");
        }

        private void CancelReport()
        {
            // Trigger an event or callback to close the view
            OnCloseView();
        }

        // Define events or callbacks for showing dialogs and closing the view
        public event Action<string> ShowErrorDialog;
        public event Action<string> ShowSuccessDialog;
        public event Action CloseView;

        protected virtual void OnShowErrorDialog(string message)
        {
            ShowErrorDialog?.Invoke(message);
        }

        protected virtual void OnShowSuccessDialog(string message)
        {
            ShowSuccessDialog?.Invoke(message);
        }

        protected virtual void OnCloseView()
        {
            CloseView?.Invoke();
        }
    }
}
