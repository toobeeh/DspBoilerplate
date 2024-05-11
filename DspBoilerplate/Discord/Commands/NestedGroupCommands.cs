using DSharpPlus.Commands;

namespace DspBoilerplate.Discord.Commands;

[Command("level1")]
public class NestedGroupCommands
{
    [Command("level2")]
    public class NestedSubGroupCommands
    {
        [Command("test")]
        public async ValueTask TestCommand(CommandContext context)
        {
            await context.RespondAsync("Command detected!");
        }
    }
}