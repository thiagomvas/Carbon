using Carbon.Core;
using Cocona;

namespace Carbon.Cli;

public class ConvertCommands
{
    private readonly FfmpegVideoConverter _converter;
    
    public ConvertCommands(FfmpegVideoConverter converter)
    {
        _converter = converter;
    }
    
    [Command( Description = "Convert from one file format to another")]
    public void Convert([Argument (Description = "Path to input file including extension")] string inputFile, 
        [Argument(Description = "Path to output file or target extension")] string outputFile, 
        [Option("Arguments to pass to converters (e.g. ffmpeg args)")] string args = "")
    {
        _converter.Convert(inputFile, outputFile, args);
    }
}