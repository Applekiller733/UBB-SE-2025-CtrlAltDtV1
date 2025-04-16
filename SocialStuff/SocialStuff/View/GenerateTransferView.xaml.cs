// <copyright file="GenerateTransferView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    /// <summary>
    /// bagati ai in el summary text
    /// </summary>
    public sealed partial class GenerateTransferView : Page
    {
        /// <summary>
        /// Gets the ViewModel associated with this view.
        /// </summary>
        public GenerateTransferViewModel ViewModel { get; }

        private IChatService chatService;
        private Page lastChat;
        private Frame rightFrame;

        public GenerateTransferView(GenerateTransferViewModel GenerateTransferViewModel, Page lastChat, Frame rightFrame, int ChatID, IChatService chatService)
        {
            // Create repository and services (this would typically be injected)
            this.chatService = chatService;

            // Initialize ViewModel
            this.ViewModel = GenerateTransferViewModel;
            this.lastChat = lastChat;
            this.rightFrame = rightFrame;
            this.InitializeComponent();
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.rightFrame.Content = this.lastChat;
        }

        private void TransferTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TransferTypeComboBox.SelectedItem != null)
            {
                string selectedValue = ((ComboBoxItem)TransferTypeComboBox.SelectedItem).Content.ToString();
                this.ViewModel.SelectedTransferType = selectedValue;

                switch (selectedValue)
                {
                    case "Transfer Money":
                        this.TitleTextBlock.Text = "Make a Transfer";
                        this.TransferButton.Content = "Transfer Money";
                        break;
                    case "Request Money":
                        this.TitleTextBlock.Text = "Request Funds";
                        this.TransferButton.Content = "Request Money";
                        break;
                    case "Split Bill":
                        this.TitleTextBlock.Text = "Split Bill";
                        this.TransferButton.Content = "Split Bill";
                        break;
                    default:
                        this.TitleTextBlock.Text = string.Empty;
                        break;
                }
            }
        }

        private void AmountTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // Empty text is allowed
            if (string.IsNullOrEmpty(args.NewText))
            {
                return;
            }

            // Check if new text matches a valid numeric pattern (digits with optional single decimal point)
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(
                args.NewText,
                @"^\d*\.?\d{0,2}$");

            // Cancel if invalid
            args.Cancel = !isValid;
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // This ensures the funds check happens when text changes through user input
            if (sender is TextBox textBox && textBox.FocusState != FocusState.Unfocused)
            {
                // The binding will update the ViewModel.AmountText which triggers CheckFunds
            }
        }

        private void CurrencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This ensures the funds check happens when currency changes
            // The binding will update ViewModel.CurrencyIndex which triggers CheckFunds
        }
    }
}
