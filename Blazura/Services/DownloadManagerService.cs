using Microsoft.JSInterop;

namespace Blazura.Services;

public class DownloadManagerService(IJSRuntime JSRuntime) : IDownloadManagerService
{
    public async Task DownloadStream(MemoryStream stream, ApplicationType applicationType, string fileName)
    {
        byte[] bytes = stream.ToArray();

        await JSRuntime.InvokeVoidAsync("downloadBase64", fileName, GetMimeType(applicationType), bytes);
    }

    public async Task OpenStreamAsBlob(MemoryStream stream, ApplicationType applicationType)
    {
        byte[] bytes = stream.ToArray();

        await JSRuntime.InvokeVoidAsync("openBase64Blob", GetMimeType(applicationType), bytes);
    }

    public async Task OpenStreamAsUrl(MemoryStream stream, ApplicationType applicationType)
    {
        byte[] bytes = stream.ToArray();
        string base64 = Convert.ToBase64String(bytes);

        await JSRuntime.InvokeVoidAsync("openBase64Url", GetMimeType(applicationType), base64);
    }

    private static string GetMimeType(ApplicationType applicationType)
    {
        return applicationType switch
        {
            ApplicationType.OctetStream => "application/octet-stream",
            ApplicationType.PDF => "application/pdf",
            ApplicationType.PNG => "image/png",
            ApplicationType.JPEG => "image/jpeg",
            _ => "application/octet-stream"
        };
    }
}
