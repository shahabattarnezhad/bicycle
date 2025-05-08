using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Interfaces.Helpers;

namespace Service.Services.Helpers;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif" };
    private const int MaxFileSize = 2 * 1024 * 1024;

    public FileService(IWebHostEnvironment env) => _env = env;

    public async Task<string> SaveFileAsync(IFormFile? file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            throw new Exception("Invalid file type. Allowed: .jpg, .jpeg, .png, .gif");

        if (file.Length > MaxFileSize)
            throw new Exception("File size exceeds 2MB.");

        var originalFolderPath = Path.Combine(_env.WebRootPath, "files");
        Directory.CreateDirectory(originalFolderPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var originalFilePath = Path.Combine(originalFolderPath, fileName);

        using (var stream = new FileStream(originalFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        //return $"/files/{fileName}";
        return fileName;
    }

    public async Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            await Task.CompletedTask;
        }
    }
}
