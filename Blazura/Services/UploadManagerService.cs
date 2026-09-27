using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace Blazura.Services;

public class UploadManagerService(IJSRuntime JSRuntime) : IUploadManagerService
{
    private readonly static string rootPath = "wwwroot";
    private readonly static string uploadPath = "uploads";
    public static long MaxFileSize { get; } = 1024L * 1024L * 1024L;

    public async Task<string> UploadAsync(IFormFile formFile, params string[] paths)
    {
        return await InternalUploadAsync(formFile.OpenReadStream(), formFile.FileName, paths);
    }

    public async Task<string> UploadAsync(IBrowserFile browserFile, params string[] paths)
    {
        return await InternalUploadAsync(browserFile.OpenReadStream(MaxFileSize), browserFile.Name, paths);
    }

    private static async Task<string> InternalUploadAsync(Stream file, string fileName, params string[] paths)
    {
        fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

        var filePath = GenerateFilePath(paths);

        GenerateDirectories(filePath);

        using var stream = File.Create(Path.Combine([rootPath, uploadPath, .. paths, fileName]));

        await file.CopyToAsync(stream);

        return $"/{Path.Combine([uploadPath, .. paths, fileName]).Replace("\\\\", "\\").Replace("\\", "/")}";
    }

    public bool Delete(string path)
    {
        var fullPath = Path.Combine(rootPath, path.TrimStart('/'));
        if (!File.Exists(fullPath)) return false;

        File.Delete(fullPath);

        var directory = Directory.GetParent(fullPath);
        if (directory is not null)
        {
            ClearEmptyDirectory(directory);
        }

        return true;
    }

    public async Task<string> GetBrowserFileAsUrl(IBrowserFile browserFile)
    {
        using var stream = new MemoryStream();
        await browserFile.OpenReadStream(MaxFileSize).CopyToAsync(stream);
        return $"data:{browserFile.ContentType};base64,{Convert.ToBase64String(stream.ToArray())}";
    }

    /// <summary>
    /// Experimental
    /// </summary>
    /// <param name="browserFile"></param>
    /// <returns></returns>
    public async Task<string> GetBrowserFileAsBlob(IBrowserFile browserFile)
    {
        using var stream = new MemoryStream();
        await browserFile.OpenReadStream(MaxFileSize).CopyToAsync(stream);

        var text = await JSRuntime.InvokeAsync<string>("convertBase64ToBlob", browserFile.ContentType, Convert.ToBase64String(stream.ToArray()));

        return text;
    }

    private static void GenerateDirectories(string filePath)
    {
        if (Directory.Exists(Path.Combine(rootPath, filePath)))
        {
            return;
        }

        if (!Directory.Exists(Path.Combine(rootPath, uploadPath)))
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            Directory.CreateDirectory(Path.Combine(rootPath, uploadPath));
        }

        Directory.CreateDirectory(Path.Combine(rootPath, filePath));
    }

    private static void ClearEmptyDirectory(DirectoryInfo directory)
    {
        if (!directory.EnumerateFileSystemInfos().Any())
        {
            directory.Delete();
            DirectoryInfo? parent = directory.Parent;
            if (parent is not null && Directory.GetParent(rootPath)?.FullName != parent.Parent?.FullName)
            {
                ClearEmptyDirectory(parent);
            }
        }
    }

    private static string GenerateFilePath(params string[] paths)
    {
        string initialUploadPath = uploadPath;
        foreach (var path in paths)
        {
            initialUploadPath = Path.Combine(initialUploadPath, path);
        }
        return initialUploadPath;
    }
}
