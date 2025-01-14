namespace Carbon.Core;

public interface IFileConverter
{
    void Convert(string inputFile, string outputFile, string args);
    bool CanConvert(string to, string from);
}