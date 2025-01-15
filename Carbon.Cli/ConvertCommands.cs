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

    [Command(Description = "Convert from one file format to another (supports batch conversion with wildcards)")]
public void Convert(
    [Argument(Description = "Path to input file or wildcard pattern (e.g., '*.webm')")] string inputPath, 
    [Argument(Description = "Path to output file (extension or directory with wildcard)")] string outputPath, 
    [Option("Arguments to pass to converters (e.g., ffmpeg args)")] string args = "")
{
    // Handle wildcard inputs
    var inputDir = Path.GetDirectoryName(inputPath) ?? Directory.GetCurrentDirectory();
    var inputPattern = Path.GetFileName(inputPath);
    var isBatchMode = inputPattern.Contains("*");

    var files = isBatchMode
        ? Directory.GetFiles(inputDir, inputPattern)
        : new[] { inputPath };

    if (!files.Any())
    {
        CliWriter.WriteError($"No files found matching pattern: {inputPath}");
        return;
    }

    // Determine if outputPath is an extension or a directory with wildcard
    var isExtension = Path.GetExtension(outputPath).TrimStart('.').Length > 0 && !outputPath.Contains("*");
    var isWildcardOutput = outputPath.Contains("*");

    foreach (var inputFile in files)
    {
        if (!File.Exists(inputFile))
        {
            CliWriter.WriteError($"Input file does not exist: {inputFile}");
            continue;
        }

        // Determine the actual output file for each input file
        string actualOutputFile;
        if (isExtension)
        {
            // Case 1: Output path is a file extension
            actualOutputFile = Path.Combine(
                inputDir, 
                Path.GetFileNameWithoutExtension(inputFile) + "." + outputPath.TrimStart('.')
            );
        }
        else if (isWildcardOutput)
        {
            // Case 2: Output path is a directory with wildcard
            var outputDir = Path.GetDirectoryName(outputPath) ?? Directory.GetCurrentDirectory();
            var outputPattern = Path.GetFileName(outputPath);
            var outputExtension = Path.GetExtension(outputPattern).TrimStart('.');

            actualOutputFile = Path.Combine(
                outputDir,
                Path.GetFileNameWithoutExtension(inputFile) + "." + outputExtension
            );
        }
        else
        {
            CliWriter.WriteError("Invalid output path format. Must be an extension or directory with wildcard.");
            return;
        }

        if (File.Exists(actualOutputFile))
        {
            CliWriter.WriteError($"Output file already exists: {actualOutputFile}");
            continue;
        }

        var from = Path.GetExtension(inputFile).TrimStart('.');
        var to = Path.GetExtension(actualOutputFile).TrimStart('.');
        var converter = _converters.FirstOrDefault(x => x.CanConvert(to, from));

        if (converter == null)
        {
            CliWriter.WriteError($"Conversion from {from} to {to} is not supported for file: {inputFile}");
            continue;
        }

        try
        {
            converter.Convert(inputFile, actualOutputFile, args);
            CliWriter.WriteSuccess($"Successfully converted {inputFile} to {actualOutputFile}");
        }
        catch (Exception ex)
        {
            CliWriter.WriteError($"Error converting file {inputFile}: {ex.Message}");
        }
    }
}

}