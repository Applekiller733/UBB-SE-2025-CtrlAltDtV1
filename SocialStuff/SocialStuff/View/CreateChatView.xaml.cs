// <copyright file="CreateChatView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SocialStuff.View
{
    using Microsoft.UI.Xaml.Controls;
    using SocialStuff.Services.Interfaces;
    using SocialStuff.ViewModel;

    public sealed partial class CreateChatView : Page
    {
        public IChatService chatService;
        public IUserService userService;

        public CreateChatView(ChatListViewModel chatListViewModel, IChatService chatService, IUserService userService)
        {
            this.InitializeComponent();
            this.chatService = chatService;
            this.userService = userService;

            this.MainGrid.DataContext = new CreateChatViewModel(chatListViewModel, chatService, userService);
        }
    }
}
