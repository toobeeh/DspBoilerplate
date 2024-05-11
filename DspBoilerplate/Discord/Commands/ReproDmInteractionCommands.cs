using DSharpPlus.Commands;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace DspBoilerplate.Discord.Commands;

public class ReproDmInteractionCommands
{
    [Command("test")]
    public async ValueTask TestCommand(CommandContext context)
    {
        var builder = new DiscordMessageBuilder().WithContent(":o");
        var btn = new DiscordButtonComponent(DiscordButtonStyle.Success, "test", "hello there");
        builder.AddComponents(btn);
        
        await context.RespondAsync(builder);
        var response = await context.GetResponseAsync() ?? throw new NullReferenceException("expected message but got null");
        var interactivity = context.Client.GetInteractivity();
        
        var btnEvent = await interactivity.WaitForButtonAsync(response, context.User);
        await btnEvent.Result.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage);
        
        builder.ClearComponents();
        builder.WithContent("general kenobi");
        await response.ModifyAsync(builder);
    }
}