using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_CHAT.Models
{
    public class ChatModel
    {
        public List<ChatUser> Users;  // Все пользователи чата

        public List<ChatMessage> Messages; // все сообщения

        public ChatModel()
        {
            Users = new List<ChatUser>();
            Messages = new List<ChatMessage>();

           
        }
    }
    public class ChatUser
    {
        public string Name;
        public DateTime LoginTime;
        public DateTime LastPing;
    }

    public class ChatMessage
    {
        // автор сообщения, если null - автор сервер
        public ChatUser User;
        // время сообщения
        public DateTime Date = DateTime.Now;
        // текст сообщения
        public string Text = "";
    }
}