namespace Properties.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(Func<Stream> streamProvider, string fileName, string contentType = "image/png", CancellationToken cancellationToken = default);

        Task<Stream?> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    }

}
