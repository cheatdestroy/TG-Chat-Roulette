﻿using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Commands
{
    /// <summary>
    /// Команда настроек
    /// </summary>
    public class ProfileCommand : ICommandBase
    {
        public string Name => "Профиль";
        public List<string> Triggers { get; set; }

        public ProfileCommand()
        {
            Triggers = new List<string>
            {
                "/profile"
            };
        }

        public async Task Execute(Update update)
        {
        }
    }
}
