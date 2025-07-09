namespace Blazura.Services;

public interface IDownloadManagerService
{
    Task DownloadStream(MemoryStream stream);
    Task OpenStreamAsBlob(MemoryStream stream);
    Task OpenStreamAsUrl(MemoryStream stream);
}
