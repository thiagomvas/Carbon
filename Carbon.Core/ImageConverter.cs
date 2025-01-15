using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Core;

public class ImageConverter : IFileConverter
{
    private readonly HashSet<string> _supportedFormats = new()
    {
        "png",
        "jpg",
        "jpeg",
        "gif",
        "bmp",
        "tiff",
        "webp"
    };

    public void Convert(string inputFile, string outputFile, string args)
    {
        using var image = Image.Load(inputFile);

        IImageEncoder encoder = GetImageEncoder(outputFile);

        image.Save(outputFile, encoder);
    }

    private IImageEncoder GetImageEncoder(string path)
    {
        var extension = Path.GetExtension(path).ToLower();
        return extension switch
        {
            ".png" => new PngEncoder(),
            ".jpg" or ".jpeg" => new JpegEncoder(),
            ".gif" => new GifEncoder(),
            ".bmp" => new BmpEncoder(),
            ".tiff" => new TiffEncoder(),
            ".webp" => new WebpEncoder(),
            _ => throw new NotSupportedException($"The format {extension} is not supported.")
        };
    }

    public bool CanConvert(string to, string from)
    {
        return _supportedFormats.Contains(to.ToLower()) && _supportedFormats.Contains(from.ToLower());
    }
}