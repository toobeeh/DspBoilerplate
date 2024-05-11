namespace DspBoilerplate.Discord;

public class DiscordConfig
{
    public required string DiscordToken { get; init; }
    public required string Prefix { get; init; }
    public required bool UseSlashCommands { get; init; }
}