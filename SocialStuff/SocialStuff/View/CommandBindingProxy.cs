// <copyright file="CommandBindingProxy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using System.Windows.Input;
    using Microsoft.UI.Xaml;

    public class CommandBindingProxy : DependencyObject
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBindingProxy), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }
    }
}