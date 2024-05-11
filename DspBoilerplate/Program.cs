using DspBoilerplate.Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DspBoilerplate;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Services
            .AddLogging(opt => opt
                .AddConfiguration(config.GetSection("Logging"))
                .AddConsole())
            .Configure<DiscordConfig>(config.GetRequiredSection("Discord"))
            .AddHostedService<DiscordBotClient>()
            .BuildServiceProvider();

        var host = builder.Build();
        await host.RunAsync();
    }
}