using TG.ChatBot.Host.Commands;
using TG.ChatBot.Host.Services;
using TG.ChatBot.Host.Services.CollectionExtension;
using TG.ChatBot.Host.Services.Communication;
using TG.ChatBot.Host.Services.StepByStep;
using TG.ChatBot.Host.Services.StepByStep.Actions;
using TG.ChatBot.Host.Services.StepsByStep.Steps;
using NLog;
using NLog.Web;
using Telegram.Bot;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain.Repository;
using TG.ChatBot.Common.Domain.Repository.Interfaces;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Domain.Context;
using Microsoft.EntityFrameworkCore;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Host.Services.StepByStep.Steps;

var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();
logger.Debug("Application started...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var botConfig = builder.Configuration.GetMainConfigurationToObject();
    var dbConfig = builder.Configuration.GetRequiredSection(ConfigurationHelper.GetNameSectionDatabaseConfiguration())["ConnectingString"];

    builder.Services.AddTransient<IUser, UsersRepository>();
    builder.Services.AddTransient<ISettings, SettingsRepository>();
    builder.Services.AddTransient<IAction, ActionsRepository>();
    builder.Services.AddTransient<RepositoryService>();

    builder.Services
        .AddHostedService<ConfigureWebHook>()
        .AddHostedService<NotifyChatRoomEvents>();

    // Добавление сервиса последовательных действий
    builder.Services.AddActionService<ActionService>();

    // Добавление шагов для действий
    builder.Services.AddActionSteps<ActionSteps>()
        .AddStep<GenderStep>()
        .AddStep<AgeStep>()
        .AddStep<PreferredChatTypeStep>()
        .AddStep<PreferredGenderStep>()
        .AddStep<PreferredAgeStep>()
        .AddStep<ProfileStep>();

    // Добавление действий
    builder.Services
        .AddAction<StartAction>()
        .AddAction<ProfileAction>();

    // Добавление конфигурации команд и сами команды
    builder.Services.AddCommandService<CommandService>()
        .AddCommand<StartCommand>()
        .AddCommand<StopCommand>()
        .AddCommand<SearchCommand>()
        .AddCommand<SkipCommand>()
        .AddCommand<ProfileCommand>();

    // Замена стандартного HttpClient
    builder.Services.AddHttpClient("tgwebhook")
        .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.Token, httpClient));

    // Контекст базы данных
    builder.Services.AddDbContext<BotDbContext>(options => options.UseSqlServer(dbConfig));

    builder.Services.AddControllers()
        .AddNewtonsoftJson();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
    var app = builder.Build();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Problem with startup...");
    throw;
}
finally
{
    LogManager.Shutdown();
}