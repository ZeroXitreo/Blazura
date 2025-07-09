using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Blazura.Services;

public interface IUploadManagerService
{
    public static long MaxFileSize { get; }
    public Task<string> UploadAsync(IFormFile formFile, string? folderName = null);
    public Task<string> UploadAsync(IBrowserFile formFile, string? folderName = null);
    public bool Delete(string path);
}
