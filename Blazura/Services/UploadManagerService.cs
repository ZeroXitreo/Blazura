using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Blazura.Services;

public class UploadManagerService : IUploadManagerService
{
    private readonly string rootPath = "wwwroot";
    private readonly string uploadPath = "uploads";
    public static long MaxFileSize => 1024L * 1024L * 1024L;

    public async Task<string> UploadAsync(IFormFile formFile, string? folderName)
    {
        string initialUploadPath = uploadPath;
        if (folderName is not null)
        {
            initialUploadPath = Path.Combine(uploadPath, folderName);
        }

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

        string filePath = initialUploadPath;

        filePath = Path.Combine(filePath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

        using (FileStream stream = File.Create(Path.Combine(rootPath, filePath)))
        {
            await formFile.CopyToAsync(stream);
        }

        filePath = filePath.Replace("\\\\", "\\");
        filePath = filePath.Replace("\\", "/");

        return $"/{filePath}";
    }

    public async Task<string> UploadAsync(IBrowserFile file, string? folderName)
    {
        string initialUploadPath = uploadPath;
        if (folderName is not null)
        {
            initialUploadPath = Path.Combine(uploadPath, folderName);
        }

        // Generate path if not exists
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

        // Process upload of file
        string filePath = initialUploadPath;

        filePath = Path.Combine(filePath, Guid.NewGuid().ToString() + Path.GetExtension(file.Name));

        using (FileStream stream = File.Create(Path.Combine(rootPath, filePath)))
        {
            //await file.CopyToAsync(stream);
            await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);
        }

        filePath = filePath.Replace("\\\\", "\\").Replace("\\", "/");

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
}
