using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DspBoilerplate.Discord.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DspBoilerplate.Discord;

public class DiscordBotClient(
    ILogger<DiscordBotClient> logger,
    IOptions<DiscordConfig> options,
    ILoggerFactory loggerFactory,
    IServiceProvider serviceProvider) : IHostedService
{
    private readonly DiscordClient _botClient = new(new DiscordConfiguration
    {
        Token = options.Value.DiscordToken,
        TokenType = TokenType.Bot,
        LoggerFactory = loggerFactory,
        Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
    });

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Discord Bot Client");


        // use commands extension
        var commands = _botClient.UseCommands(new CommandsConfiguration
        {
            ServiceProvider = serviceProvider,
            UseDefaultCommandErrorHandler = true,
            RegisterDefaultCommandProcessors = false
        });

        // create text processor
        var textCommandProcessor = new TextCommandProcessor
        {
            Configuration = new TextCommandConfiguration
            {
                PrefixResolver = new DefaultPrefixResolver(options.Value.Prefix).ResolvePrefixAsync
            }
        };
        await commands.AddProcessorsAsync(textCommandProcessor);

        // create slash processor, if configured
        if (options.Value.UseSlashCommands)
        {
            var slashCommandProcessor = new SlashCommandProcessor();
            await commands.AddProcessorAsync(slashCommandProcessor);
        }

        // add command modules
        commands.AddCommands(typeof(BoilerplateCommands));

        await _botClient.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Discord Bot Client");
        await _botClient.DisconnectAsync();
    }
}