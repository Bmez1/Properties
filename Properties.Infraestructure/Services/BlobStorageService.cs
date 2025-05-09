namespace Properties.Infraestructure.Services;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.Extensions.Configuration;

using Properties.Application.Interfaces;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:BlobStorage:ConnectionString"];
        var containerName = configuration["Azure:BlobStorage:ContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadFileAsync(Func<Stream> streamProvider, string fileName, string contentType = "image/png", CancellationToken cancellationToken = default)
    {
        using var stream = streamProvider();
        var blobClient = _containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeader
        }, cancellationToken);

        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            return null;

        var response = await blobClient.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }

}

