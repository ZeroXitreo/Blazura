using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Blazura.Services;

public class UploadManagerService : IUploadManagerService
{
    private readonly string rootPath = "wwwroot";
    private readonly string uploadPath = "uploads";
    public static long MaxFileSize => 1024L * 1024L * 1024L;

    public async Task<string> UploadAsync(IFormFile formFile, params string[] paths)
    {
        var filePath = GenerateFilePath(paths);

        GenerateDirectories(filePath);

        filePath = Path.Combine(filePath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

        using (FileStream stream = File.Create(Path.Combine(rootPath, filePath)))
        {
            await formFile.CopyToAsync(stream);
        }

        filePath = filePath.Replace("\\\\", "\\");
        filePath = filePath.Replace("\\", "/");

        return $"/{filePath}";
    }

    public async Task<string> UploadAsync(IBrowserFile file, params string[] paths)
    {
        var filePath = GenerateFilePath(paths);

        GenerateDirectories(filePath);

        filePath = Path.Combine(filePath, Guid.NewGuid().ToString() + Path.GetExtension(file.Name));

        using (FileStream stream = File.Create(Path.Combine(rootPath, filePath)))
        {
            await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);
        }

        filePath = filePath.Replace("\\\\", "\\");
        filePath = filePath.Replace("\\", "/");

        return $"/{filePath}";
    }

    public bool Delete(string path)
    {
        path = Path.Combine(rootPath, path.TrimStart('/'));
        if (File.Exists(path))
        {
            File.Delete(path);
            DirectoryInfo? directory = Directory.GetParent(path);
            if (directory is not null)
            {
                ClearEmptyDirectory(directory);
            }
            return true;
        }

        return false;
    }

    public async Task<string> GetBrowserFileAsUrl(IBrowserFile browserFile)
    {
        using MemoryStream stream = new();
        await browserFile.OpenReadStream(MaxFileSize).CopyToAsync(stream);
        return $"data:{browserFile.ContentType};base64,{Convert.ToBase64String(stream.ToArray())}";
    }

    private void GenerateDirectories(string initialUploadPath)
    {
        if (!Directory.Exists(Path.Combine(rootPath, initialUploadPath)))
        {
            if (!Directory.Exists(Path.Combine(rootPath, uploadPath)))
            {
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                Directory.CreateDirectory(Path.Combine(rootPath, uploadPath));
            }

            Directory.CreateDirectory(Path.Combine(rootPath, initialUploadPath));
        }
    }

    private void ClearEmptyDirectory(DirectoryInfo directory)
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

    private string GenerateFilePath(params string[] paths)
    {
        string initialUploadPath = uploadPath;
        foreach (var path in paths)
        {
            initialUploadPath = Path.Combine(uploadPath, path);
        }
        return initialUploadPath;
    }
}
