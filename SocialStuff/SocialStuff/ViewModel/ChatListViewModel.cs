using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SocialStuff.Model.MessageClasses;
using MvvmHelpers;
using SocialStuff.Services;
using Windows.ApplicationModel.Chat;


namespace SocialStuff.ViewModel
{
    public class ChatListViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();


    }

}
