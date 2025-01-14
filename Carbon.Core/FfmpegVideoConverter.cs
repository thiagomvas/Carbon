using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Carbon.Core;

public class FfmpegVideoConverter : IFileConverter
{
    private readonly HashSet<string> _supportedFormats = new HashSet<string>
    {
        "mp4",
        "avi",
        "mov",
        "mkv",
        "flv",
        "wmv",
        "webm"
    };

    private readonly ILogger<FfmpegVideoConverter>? _logger;

    public FfmpegVideoConverter(ILogger<FfmpegVideoConverter> logger)
    {
        _logger = logger;
    }

    public void Convert(string inputFile, string outputFile, string args)
    {
        _logger?.LogInformation($"Converting {inputFile} to {outputFile}");
        ConvertFiles(inputFile, outputFile, args);
    }

    public bool CanConvert(string to, string from)
    {
        // Trim '.' from the file extensions
        to = to.TrimStart('.');
        from = from.TrimStart('.');
        return _supportedFormats.Contains(to) && _supportedFormats.Contains(from);
    }

    private void ConvertFiles(string inputFilePath, string outputFilePath, string ffmpegArgs = "")
    {
        _logger?.LogDebug("Starting ffmpeg process");
        using Process ffmpegProcess = new Process();
        ffmpegProcess.StartInfo.FileName = "ffmpeg"; // Ensure ffmpeg is in your PATH
        ffmpegProcess.StartInfo.Arguments =
            string.IsNullOrWhiteSpace(ffmpegArgs)
                ? $"-i \"{inputFilePath}\" -c:v libx264 -crf 23 -preset medium -c:a aac -b:a 128k -progress pipe:2 -nostats \"{outputFilePath}\""
                : ffmpegArgs;
        ffmpegProcess.StartInfo.RedirectStandardOutput = true;
        ffmpegProcess.StartInfo.RedirectStandardError = true;
        ffmpegProcess.StartInfo.UseShellExecute = false;
        ffmpegProcess.StartInfo.CreateNoWindow = true;

        // Event handlers for real-time logging
        ffmpegProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                // Extract and display progress updates
                if (args.Data.Contains("frame=") || args.Data.Contains("time="))
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    string data = args.Data
                        .Replace("out_time=", "Current Timestamp: ")
                        .Replace("frame=", "Frame: ");

                    // Ensure the data fits within the current console width
                    int maxWidth = Console.WindowWidth - 1; // Prevent wrapping
                    if (data.Length > maxWidth)
                    {
                        data = data.Substring(0, maxWidth);
                    }
                    else
                    {
                        data = data.PadRight(maxWidth); // Fill remaining space to clear any residual text
                    }

                    Console.Write(data); // Overwrite the same line
                }
                else
                {
                    _logger?.LogDebug("FFmpeg stderr: {data}", args.Data);
                }
            }
        };

        ffmpegProcess.Start();
        ffmpegProcess.BeginErrorReadLine(); // Start reading from the error stream


        // Wait for the process to exit
        ffmpegProcess.WaitForExit();

        _logger?.LogDebug("ffmpeg process exited with code {exitCode}", ffmpegProcess.ExitCode);

        // Check if the process failed
        if (ffmpegProcess.ExitCode != 0)
        {
            throw new InvalidOperationException($"FFmpeg process failed with exit code {ffmpegProcess.ExitCode}.");
        }

        _logger?.LogInformation("FFmpeg completed successfully.");
    }
}