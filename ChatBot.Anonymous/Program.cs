using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Models;
using ChatBot.Anonymous.Commands;
using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Services.CollectionExtension;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Domain.Repository;
using ChatBot.Anonymous.Commands.Actions;
using ChatBot.Anonymous.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetMainConfigurationToObject();
var dbConfig = builder.Configuration.GetRequiredSection(ConfigurationHelper.GetNameSectionDatabaseConfiguration())["ConnectingString"];

builder.Services.AddTransient<IUser, UsersRepository>();
builder.Services.AddTransient<ISettings, SettingsRepository>();
builder.Services.AddTransient<IAction, ActionsRepository>();
builder.Services.AddTransient<RepositoryService>();

// Добавление конфигурации вебхука
builder.Services.AddHostedService<ConfigureWebHook>();

builder.Services.AddActionService<ActionService>()
    .AddAction<StartAction>();

// Добавление конфигурации команд и сами команды
builder.Services.AddCommandService<CommandService>()
    .AddCommand<StartCommand>()
    .AddCommand<ProfileCommand>()
    .AddCommand<TestCommand>();

// Замена стандартного HttpClient
builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.Token, httpClient));

// Контекст базы данных
builder.Services.AddDbContext<BotDbContext>(options => options.UseSqlServer(dbConfig));

builder.Services.AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
