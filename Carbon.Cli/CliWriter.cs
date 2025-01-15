using Carbon.Core;

namespace Carbon.Cli;


public class CliWriter : ICliWriter
{
    public void WriteSuccess(string message, params object[]? args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[SUCCESS]");
        Console.ResetColor();
        Console.WriteLine($" {message}", args);
    }
    
    public void WriteError(string message, params object[]? args)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[ERROR]");
        Console.ResetColor();
        Console.WriteLine($" {message}", args);
    }
    
    public static void WriteWarning(string message, params object[]? args)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[WARNING]");
        Console.ResetColor();
        Console.WriteLine($" {message}", args);
    }
    
    public void WriteInfo(string message, params object[]? args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("[INFO]");
        Console.ResetColor();
        Console.WriteLine($" {message}", args);
    }
    
    public void WriteDebug(string message, params object[]? args)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[DEBUG]");
        Console.ResetColor();
        Console.WriteLine($" {message}", args);
    }
    
    
    
}