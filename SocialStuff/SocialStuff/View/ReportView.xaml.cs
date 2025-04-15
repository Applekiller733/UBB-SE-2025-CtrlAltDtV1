using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SocialStuff.Model;
using SocialStuff.Services.Implementations;
using SocialStuff.Services.Interfaces;
using SocialStuff.ViewModel;
using System;

namespace SocialStuff.Views
{
    public sealed partial class ReportView : Window
    {
        public ReportViewModel ViewModel { get; }

        public ReportView(IUserService userService, IReportService reportService, int reportedUserId, int messageId)
        {
            this.InitializeComponent();

            ViewModel = new ReportViewModel(userService, reportService, reportedUserId, messageId);
            ViewModel.ShowErrorDialog += OnShowErrorDialog;
            ViewModel.ShowSuccessDialog += OnShowSuccessDialog;
            ViewModel.CloseView += OnCloseView;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsOtherCategorySelected))
            {
                UpdateOtherReasonVisibility();
            }
        }

        private void UpdateOtherReasonVisibility()
        {
            if (ViewModel.IsOtherCategorySelected)
            {
                OtherReasonLabel.Visibility = Visibility.Visible;
                OtherReasonTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                OtherReasonLabel.Visibility = Visibility.Collapsed;
                OtherReasonTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private async void OnShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void OnShowSuccessDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
            this.Close();
        }

        private void OnCloseView()
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // reportcommand
            ViewModel.SubmitCommand.Execute(null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelCommand.Execute(null);
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Get the string content from the ComboBoxItem
                ViewModel.SelectedCategory = selectedItem.Content.ToString();
            }
        }

        private void OtherReasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.OtherReason = OtherReasonTextBox.Text;
        }
    }
}