namespace Blazura.Services;

public interface IDownloadManagerService
{
    Task DownloadStream(MemoryStream stream, ApplicationType applicationType, string fileName);
    Task OpenStreamAsBlob(MemoryStream stream, ApplicationType applicationType);
    Task OpenStreamAsUrl(MemoryStream stream, ApplicationType applicationType);
}

public enum ApplicationType
{
    OctetStream,
    PDF,
    PNG,
    JPEG,
}
