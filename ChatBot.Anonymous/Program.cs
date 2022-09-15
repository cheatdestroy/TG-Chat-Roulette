using ChatBot.Anonymous.Models;
using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Repository.Commands;
using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Services.CollectionExtension;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

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

builder.Services.AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
