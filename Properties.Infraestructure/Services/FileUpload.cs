using Microsoft.AspNetCore.Http;

using Properties.Application.Interfaces;

using System.Text.RegularExpressions;

namespace Properties.Infraestructure.Services;

public class FileUpload : IFileUpload
{
    private readonly IFormFile _file;

    public FileUpload(IFormFile file)
    {
        _file = file ?? throw new ArgumentNullException(nameof(file));
        Extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);

        var sanitizedFileName = Regex.Replace(originalFileName, @"[^a-zA-Z0-9_\-]", "");
        FileName = $"{sanitizedFileName}{Extension}";

        Size = file.Length;
    }

    public string FileName { get; }
    public string Extension { get; }
    public long Size { get; }

    public Stream OpenReadStream()
    {
        return _file.OpenReadStream();
    }
}
