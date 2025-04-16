// <copyright file="MessageTemplateSelector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Data;
    using SocialStuff.Model.MessageClasses;

    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextMessageTemplateLeft { get; set; }

        public DataTemplate TextMessageTemplateRight { get; set; }

        public DataTemplate ImageMessageTemplateLeft { get; set; }


        public DataTemplate ImageMessageTemplateRight { get; set; }

        public DataTemplate TransferMessageTemplateLeft { get; set; }

        public DataTemplate TransferMessageTemplateRight { get; set; }

        public DataTemplate RequestMessageTemplateLeft { get; set; }

        public DataTemplate RequestMessageTemplateRight { get; set; }

        public int CurrentUserID { get; set; }

        public MessageTemplateSelector()
        {
            Repository repo = new Repository();      // THIS MIGHT EXPLODE IF REPOSITORY USERID IS NOT STATIC, ILL FIGURE IT OUT
            this.CurrentUserID = repo.GetLoggedInUserID();
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Message message)
            {
                switch (message)
                {
                    case TextMessage _:
                        return message.GetSenderID() == this.CurrentUserID ? this.TextMessageTemplateRight : this.TextMessageTemplateLeft;

                    case ImageMessage _:
                        return message.GetSenderID() == this.CurrentUserID ? this.ImageMessageTemplateRight : this.ImageMessageTemplateLeft;

                    case TransferMessage _:
                        return message.GetSenderID() == this.CurrentUserID ? this.TransferMessageTemplateRight : this.TransferMessageTemplateLeft;

                    case RequestMessage _:
                        return message.GetSenderID() == this.CurrentUserID ? this.RequestMessageTemplateRight : this.RequestMessageTemplateLeft;
                }
            }

            return this.TextMessageTemplateLeft;
        }
    }
}