using DSharpPlus.Commands;

namespace DspBoilerplate.Discord.Commands;

public class BoilerplateCommands
{
    [Command("ping")]
    public async ValueTask PingCommand(CommandContext context)
    {
        await context.RespondAsync("Pong!");
    }
}