namespace Carbon.Cli;

public class CliWriter
{
    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[SUCCESS]");
        Console.ResetColor();
        Console.WriteLine(message);
    }
    
    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[ERROR]");
        Console.ResetColor();
        Console.WriteLine(message);
    }
    
    public static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[WARNING]");
        Console.ResetColor();
        Console.WriteLine(message);
    }
}