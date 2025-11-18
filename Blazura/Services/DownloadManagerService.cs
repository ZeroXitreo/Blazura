using Microsoft.JSInterop;

namespace Blazura.Services;

public class DownloadManagerService(IJSRuntime JSRuntime) : IDownloadManagerService
{
    public async Task DownloadStream(MemoryStream stream)
    {
        byte[] bytes = stream.ToArray();

        await JSRuntime.InvokeVoidAsync("downloadBase64", "file.pdf", "application/octet-stream", bytes);
    }

    public async Task OpenStreamAsBlob(MemoryStream stream)
    {
        byte[] bytes = stream.ToArray();

        await JSRuntime.InvokeVoidAsync("openBase64Blob", "application/pdf", bytes);
    }

    public async Task OpenStreamAsUrl(MemoryStream stream)
    {
        string base64 = Convert.ToBase64String(stream.ToArray());

        await JSRuntime.InvokeVoidAsync("openBase64Url", "application/pdf", base64);
    }
}
