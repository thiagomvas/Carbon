using Carbon.Core;
using Cocona;

namespace Carbon.Cli;

public class ConvertCommands
{
    private readonly ICliWriter _cliWriter;
    private readonly IEnumerable<IFileConverter> _converters;
    
    public ConvertCommands(ICliWriter writer, IEnumerable<IFileConverter> converters)
    {
        _cliWriter = writer;
        _converters = converters;
    }
    
    [Command( Description = "Convert from one file format to another")]
    public void Convert([Argument (Description = "Path to input file including extension")] string inputFile, 
        [Argument(Description = "Path to output file or target extension")] string outputFile, 
        [Option("Arguments to pass to converters (e.g. ffmpeg args)")] string args = "")
    {
        var from = Path.GetExtension(inputFile).TrimStart('.');
        var to = Path.GetExtension(outputFile).TrimStart('.');
        var converter = _converters.FirstOrDefault(x => x.CanConvert(to, from));
        if (converter == null)
        {
            _cliWriter.WriteError("No converter found for {0} to {1}", from, to);
            return;
        }
        
        converter.Convert(inputFile, outputFile, args);
    }
}