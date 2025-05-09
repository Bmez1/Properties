namespace Properties.Application.Interfaces
{
    public interface IFileUpload
    {
        string FileName { get; }
        string Extension { get; }
        long Size { get; }

        Stream OpenReadStream();
    }

}
