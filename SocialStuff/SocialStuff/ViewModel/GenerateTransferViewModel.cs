// <copyright file="GenerateTransferViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using SocialStuff.Services.Interfaces;

    public class GenerateTransferViewModel : INotifyPropertyChanged
    {
        private string amountText;
        private string description;
        private string selectedTransferType;
        private int transferTypeIndex = -1;
        private int currencyIndex = -1;
        private bool isFormValid;
        private bool hasSufficientFunds = true;
        private bool isCheckingFunds = false;
        private int ChatID;

        private readonly IChatService chatService;

        public GenerateTransferViewModel(IChatService chatService,int ChatID)
        {
            this.chatService = chatService;
            this.ChatID = ChatID;
            this.SendMessageCommand = new RelayCommand(this.ExecuteSendMessage);

            // Set default values
            this.Description = string.Empty;
            this.AmountText = string.Empty;
            this.SelectedTransferType = string.Empty;
        }

        public string AmountText
        {
            get => this.amountText;
            set
            {
                this.amountText = value;
                this.OnPropertyChanged();
                this.ValidateForm();
                this.CheckFunds();
            }
        }

        public float Amount
        {
            get
            {
                if (float.TryParse(this.AmountText, out float result))
                {
                    return result;
                }

                return 0f;
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.OnPropertyChanged();
                this.ValidateForm();
            }
        }

        public string SelectedTransferType
        {
            get => this.selectedTransferType;
            set
            {
                this.selectedTransferType = value;
                this.OnPropertyChanged();
                this.ValidateForm();
                this.CheckFunds();
            }
        }

        public int TransferTypeIndex
        {
            get => this.transferTypeIndex;
            set
            {
                this.transferTypeIndex = value;
                this.OnPropertyChanged();

                // Update the SelectedTransferType based on index
                switch (this.transferTypeIndex)
                {
                    case 0:
                        this.SelectedTransferType = "Transfer Money";
                        break;
                    case 1:
                        this.SelectedTransferType = "Request Money";
                        break;
                    case 2:
                        this.SelectedTransferType = "Split Bill";
                        break;
                    default:
                        this.SelectedTransferType = null;
                        break;
                }

                this.ValidateForm();
            }
        }

        public int CurrencyIndex
        {
            get => this.currencyIndex;
            set
            {
                this.currencyIndex = value;
                this.OnPropertyChanged();
                this.ValidateForm();
                this.CheckFunds();
            }
        }

        public string Currency
        {
            get
            {
                switch (this.CurrencyIndex)
                {
                    case 0:
                        return "USD";
                    case 1:
                        return "EUR";
                    case 2:
                        return "RON";
                    default:
                        return null;
                }
            }
        }

        public bool IsFormValid
        {
            get => this.isFormValid;
            set
            {
                if (this.isFormValid != value)
                {
                    this.isFormValid = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ICommand SendMessageCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ValidateForm()
        {
            this.IsFormValid =
                this.TransferTypeIndex >= 0 &&
                this.CurrencyIndex >= 0 &&
                !string.IsNullOrWhiteSpace(this.AmountText) &&
                float.TryParse(this.AmountText, out float parsedAmount) &&
                parsedAmount > 0 &&
                (this.SelectedTransferType != "Transfer Money" || this.HasSufficientFunds);
        }

        private void ExecuteSendMessage()
        {
            try
            {
                switch (this.SelectedTransferType)
                {
                    case "Transfer Money":
                        this.chatService.SendMoneyViaChat(this.Amount, this.Currency, this.Description, this.ChatID);
                        break;
                    case "Request Money":
                        this.chatService.RequestMoneyViaChat(this.Amount, this.Currency, this.ChatID, this.Description);
                        break;
                    case "Split Bill":
                        float SplitAmount = this.Amount / (this.chatService.GetNumberOfParticipants(this.ChatID));
                        this.chatService.RequestMoneyViaChat(SplitAmount, this.Currency, this.ChatID, this.description);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid transfer type selected.");
                }

                // Reset form after successful operation
                this.ResetForm();
            }
            catch (Exception ex)
            {
                // Handle exception (in a real app, you would show a message to the user)
                System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            this.AmountText = string.Empty;
            this.Description = string.Empty;
            this.HasSufficientFunds = true;

            // Optionally reset other fields if needed
        }

        public bool HasSufficientFunds
        {
            get => this.hasSufficientFunds;
            set
            {
                if (this.hasSufficientFunds != value)
                {
                    this.hasSufficientFunds = value;
                    this.OnPropertyChanged();
                    this.OnPropertyChanged(nameof(this.ShowInsufficientFundsError));
                }
            }
        }

        public bool IsCheckingFunds
        {
            get => this.isCheckingFunds;
            set
            {
                if (this.isCheckingFunds != value)
                {
                    this.isCheckingFunds = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool ShowInsufficientFundsError
        {
            get
            {
                // Only show error for Transfer Money operations
                return !this.HasSufficientFunds &&
                       this.SelectedTransferType == "Transfer Money" &&
                       !string.IsNullOrWhiteSpace(this.AmountText) &&
                       float.TryParse(this.AmountText, out float amount) &&
                       amount > 0 &&
                       this.CurrencyIndex >= 0;
            }
        }

        private void CheckFunds()
        {
            // Only check funds for transfer money operations
            if (this.SelectedTransferType != "Transfer Money")
            {
                this.HasSufficientFunds = true;
                return;
            }

            // Return if any required fields are not set
            if (string.IsNullOrWhiteSpace(this.AmountText) ||
                !float.TryParse(this.AmountText, out float amount) ||
                amount <= 0 ||
                this.CurrencyIndex < 0)
            {
                this.HasSufficientFunds = true;
                return;
            }

            this.IsCheckingFunds = true;

            try
            {
                int chatID = this.ChatID;
                int currentUserID = this.chatService.GetCurrentUserID();

                // Calculate total amount based on number of participants
                int participantCount = this.chatService.GetNumberOfParticipants(chatID);
                float totalAmount = amount * (participantCount - 1);

                // Check if user has enough funds for the total amount
                this.HasSufficientFunds = this.chatService.EnoughFunds(totalAmount, this.Currency, currentUserID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking funds: {ex.Message}");
                this.HasSufficientFunds = false;
            }
            finally
            {
                this.IsCheckingFunds = false;
                this.OnPropertyChanged(nameof(this.ShowInsufficientFundsError));
                this.ValidateForm();
            }
        }
    }
}