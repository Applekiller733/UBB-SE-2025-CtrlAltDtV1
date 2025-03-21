using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Graphics;
using WinRT.Interop;



namespace SocialStuff.View
{
    public sealed partial class GenerateRequest : Window
    {
        public GenerateRequest()
        {
            this.InitializeComponent();


            TransferButton.IsEnabled = false;

            // Add event handlers for validation
            TransferTypeComboBox.SelectionChanged += ValidateFields;
            CurrencyComboBox.SelectionChanged += ValidateFields;
            AmountTextBox.TextChanged += ValidateFields;
        }

        private void ValidateFields(object sender, object e)
        {
            // Check if all required fields are filled
            bool isValid =
                TransferTypeComboBox.SelectedItem != null &&
                CurrencyComboBox.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(AmountTextBox.Text) &&
                decimal.TryParse(AmountTextBox.Text, out decimal parsedAmount) &&
                parsedAmount != 0.00m && parsedAmount != 0.0m && parsedAmount != 0m; // Ensure amount is a valid number

            // Enable or disable the transfer button based on validation
            TransferButton.IsEnabled = isValid;
        }



        private void TransferTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransferTypeComboBox.SelectedItem != null)
            {
                string selectedValue = ((ComboBoxItem)TransferTypeComboBox.SelectedItem).Content.ToString();
                switch (selectedValue)
                {
                    case "Transfer Money":
                        TitleTextBlock.Text = "Make a Transfer";
                        break;
                    case "Request Money":
                        TitleTextBlock.Text = "Request Funds";
                        break;
                    case "Split Bill":
                        TitleTextBlock.Text = "Split Bill";
                        break;
                    default:
                        TitleTextBlock.Text = ""; 
                        break;
                }


            }
        }


        private void AmountTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // Empty text is allowed
            if (string.IsNullOrEmpty(args.NewText))
                return;

            // Check if new text matches a valid numeric pattern (digits with optional single decimal point)
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(
                args.NewText,
                @"^\d*\.?\d{0,2}$"
             );

            // Cancel if invalid
            args.Cancel = !isValid;
        }



    }
}
