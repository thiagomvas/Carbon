using Carbon.Core;
using Cocona;
using Microsoft.Extensions.Logging;

namespace Carbon.Cli;

public class ConvertCommands
{
    private readonly IEnumerable<IFileConverter> _converters;
    
    public ConvertCommands(IEnumerable<IFileConverter> converters)
    {
        _converters = converters;
    }
    
    [Command( Description = "Convert from one file format to another")]
    public void Convert([Argument (Description = "Path to input file including extension")] string inputFile, 
        [Argument(Description = "Path to output file or target extension")] string outputFile, 
        [Option("Arguments to pass to converters (e.g. ffmpeg args)")] string args = "")
    {
        
        if (!File.Exists(inputFile))
        {
            CliWriter.WriteError("Input file does not exist");
            return;
        }
        
        if(File.Exists(outputFile))
        {
            CliWriter.WriteError("Output file already exists");
            return;
        }

        // check if output file is just the result extension, if it is, save to current executing directory
        if (Path.GetExtension(outputFile) == "" || outputFile.TrimStart('.') == Path.GetExtension(outputFile))
        {
            outputFile = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(inputFile) + "." + outputFile.TrimStart('.'));
        }
        
        
        
        var from = Path.GetExtension(inputFile).TrimStart('.');
        var to = Path.GetExtension(outputFile).TrimStart('.');
        var converter = _converters.FirstOrDefault(x => x.CanConvert(to, from));
        if (converter == null)
        {
            CliWriter.WriteError($"Conversion from {from} to {to} is not supported");
            return;
        }

        try
        {
            converter.Convert(inputFile, outputFile, args);
        }
        catch (Exception e)
        {
            CliWriter.WriteError("An error occurred while converting the file");
            throw;
        }
        
        CliWriter.WriteSuccess($"Successfully saved converted file as {outputFile}");
    }
}