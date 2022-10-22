﻿using Telegram.Bot.Types;
using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Host.Services.StepByStep.Interfaces
{
    public interface IAction
    {
        /// <summary>
        /// Идентификатор действия
        /// </summary>
        CommandActions Action { get; }

        /// <summary>
        /// Шаги действия
        /// </summary>
        IActionSteps Steps { get; }

        /// <summary>
        /// Вызывает пошаговую инициализацию
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task ExecuteSteps(Message message, Common.Domain.Entities.User user);

        /// <summary>
        /// Обрабатывает пошаговое получение данных
        /// </summary>
        /// <param name="update"></param>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task ProcessingSteps(Update update, Common.Domain.Entities.User user);

        /// <summary>
        /// Завершает выполнение текущего действия
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <returns></returns>
        Task FinishAction(Message message, long userId);
    }
}
