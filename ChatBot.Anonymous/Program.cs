using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Models;
using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Commands;
using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Services.CollectionExtension;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
var dbConfig = builder.Configuration.GetConnectionString("ConnectingString");

// Добавление конфигурации вебхука
builder.Services.AddHostedService<ConfigureWebHook>();

// Добавление команд в общий пул
builder.Services.AddCommands(
    new List<ICommandBase>
    {
        new SettingsCommand(),
        new TestCommand()
    }
);

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
