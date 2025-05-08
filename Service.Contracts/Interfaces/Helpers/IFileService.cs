using Microsoft.AspNetCore.Http;

namespace Service.Contracts.Interfaces.Helpers;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);

    Task DeleteFileAsync(string filePath);
}
