using TG.ChatBot.Host.Commands;
using TG.ChatBot.Host.Common.Helpers;
using TG.ChatBot.Host.Domain.Context;
using TG.ChatBot.Host.Domain.Repository;
using TG.ChatBot.Host.Domain.Repository.Interfaces;
using TG.ChatBot.Host.Models.Interfaces;
using TG.ChatBot.Host.Services;
using TG.ChatBot.Host.Services.CollectionExtension;
using TG.ChatBot.Host.Services.Communication;
using TG.ChatBot.Host.Services.StepByStep;
using TG.ChatBot.Host.Services.StepByStep.Actions;
using TG.ChatBot.Host.Services.StepByStep.Actions.StartAction;
using TG.ChatBot.Host.Services.StepsByStep.Steps;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Telegram.Bot;

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

    builder.Services.AddSingleton<IChatHub, ChatHub>();

    builder.Services
        .AddHostedService<ConfigureWebHook>();

    // Добавление сервиса последовательных действий
    builder.Services.AddActionService<ActionService>();

    // Добавление шагов для действий
    builder.Services
        .AddStep<GenderStep>()
        .AddStep<AgeStep>()
        .AddStep<PreferredChatTypeStep>()
        .AddStep<PreferredGenderStep>()
        .AddStep<PreferredAgeStep>();

    // Добавление последовательности шагов для определенных действий
    builder.Services
        .AddActionSteps<StartActionSteps>();

    // Добавление действий
    builder.Services
        .AddAction<StartAction<StartActionSteps>>();

    // Добавление конфигурации команд и сами команды
    builder.Services.AddCommandService<CommandService>()
        .AddCommand<StartCommand>()
        .AddCommand<StopCommand>()
        .AddCommand<SearchCommand>()
        .AddCommand<SkipCommand>();

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