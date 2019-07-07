using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Test_CHAT.Models;

namespace Test_CHAT.Controllers
{
    public class SharedController : Controller
    {
        public static ChatModel chatModel;

        public ActionResult Nick(string user, bool? logOn, bool? logOff, string chatMessage)
        {
            if (chatModel == null)
                chatModel = new ChatModel();

            //оставляем только последние 90 сообщений
            if (chatModel.Messages.Count > 100)
                chatModel.Messages.RemoveRange(0, 90);

            // если обычный запрос, просто возвращаем представление
            if (!Request.IsAjaxRequest())
            {
                return View(chatModel);
            }
            else if (logOn != null && (bool)logOn)
            {
                //проверяем, существует ли уже такой пользователь
                if (chatModel.Users.FirstOrDefault(u => u.Name == user) != null)
                {
                    throw new Exception("We have already the user with such nick!");
                }
                else if (chatModel.Users.Count > 10)
                {
                    throw new Exception("Chat is full");
                }
                else
                {
                    // добавляем в список нового пользователя
                    chatModel.Users.Add(new ChatUser()
                    {
                        Name = user,
                        LoginTime = DateTime.Now,
                        LastPing = DateTime.Now
                    });

                    // добавляем в список ссообщений сообщение о новом пользователе
                    chatModel.Messages.Add(new ChatMessage()
                    {
                        Text = user + " is in chat.",
                        Date = DateTime.Now
                    });
                }

                return PartialView("ChatRoom", chatModel);
            }
            else if (logOff != null && (bool)logOff)
            {
                LogOff(chatModel.Users.FirstOrDefault(u => u.Name == user));
                return PartialView("ChatRoom", chatModel);
            }
            else
            {
                ChatUser currentUser = chatModel.Users.FirstOrDefault(u => u.Name == user);

                //для каждлого пользователя запоминаем воемя последнего обновления
                //currentUser.LastPing = DateTime.Now;

                // удаляем неаквтивных пользователей
                List<ChatUser> removeThese = new List<ChatUser>();
                foreach (Models.ChatUser usr in chatModel.Users)
                {
                    TimeSpan span = DateTime.Now - usr.LastPing;
                    if (span.TotalSeconds > 1000)
                        removeThese.Add(usr);
                }
                foreach (ChatUser u in removeThese)
                {
                    LogOff(u);
                }

                // добавляем в список сообщений новое сообщение
                if (!string.IsNullOrEmpty(chatMessage))
                {
                    chatModel.Messages.Add(new ChatMessage()
                    {
                        User = currentUser,
                        Text = chatMessage,
                        Date = DateTime.Now
                    });
                }

                return PartialView("History", chatModel);
            }

        }

        // при выходе пользователя удаляем его из списка

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(ChatUser user)
        {
            chatModel.Users.Remove(user);
            chatModel.Messages.Add(new ChatMessage()
            {
                Text = user.Name + " leaved the chat.",
                Date = DateTime.Now
            });
           
            return RedirectToAction("Index", "Home");
        }

        
    }
}