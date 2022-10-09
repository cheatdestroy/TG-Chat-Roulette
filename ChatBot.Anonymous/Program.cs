using ChatBot.Anonymous.Commands;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Domain.Repository;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Services.CollectionExtension;
using ChatBot.Anonymous.Services.StepByStep;
using ChatBot.Anonymous.Services.StepByStep.Actions;
using ChatBot.Anonymous.Services.StepByStep.Actions.StartAction;
using ChatBot.Anonymous.Services.StepsByStep.Steps;
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

    // Добавление конфигурации вебхука
    builder.Services.AddHostedService<ConfigureWebHook>();

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
        .AddCommand<ProfileCommand>()
        .AddCommand<TestCommand>();

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