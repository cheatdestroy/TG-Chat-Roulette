﻿using ChatBot.Anonymous.Common.Enums;

namespace ChatBot.Anonymous.Services.StepByStep.Interfaces
{
    public interface IStep
    {
        Step Id { get; }
        Task Execute(long chatId);
        Task Processing(string data, long userId);
    }
}