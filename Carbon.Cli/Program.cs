using Carbon.Cli;
using Carbon.Core;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddTransient<IFileConverter, FfmpegVideoConverter>();

#if DEBUG
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});
#endif

var app = builder.Build();
app.AddSubCommand("convert", x => x.AddCommands<ConvertCommands>());

app.Run();