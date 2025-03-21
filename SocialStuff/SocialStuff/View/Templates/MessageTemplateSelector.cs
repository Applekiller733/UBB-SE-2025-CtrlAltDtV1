using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using SocialStuff.Model.MessageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialStuff.View.Templates
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextMessageTemplate { get; set; }
        public DataTemplate ImageMessageTemplate { get; set; }
        public DataTemplate TransferMessageTemplate { get; set; }
        public DataTemplate RequestMessageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (item)
            {
                case TextMessage _: return TextMessageTemplate;
                case ImageMessage _: return ImageMessageTemplate;
                case TransferMessage _: return TransferMessageTemplate;
                case RequestMessage _: return RequestMessageTemplate;
                default: return TextMessageTemplate;
            }
        }
    }
}
