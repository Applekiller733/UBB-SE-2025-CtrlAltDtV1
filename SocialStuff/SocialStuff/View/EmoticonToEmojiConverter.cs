// <copyright file="EmoticonToEmojiConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class EmoticonToEmojiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string message)
            {
                return EmoticonConverter.ConvertEmoticonsToEmojis(message);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Usually, you don't need to convert back from emoji to emoticon
            return value;
        }
    }
}
