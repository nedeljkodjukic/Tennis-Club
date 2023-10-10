namespace TennisClub.Api.Services;

public class ImageStoreService
{
    private readonly string _baseImagePath;
    private readonly string _imageDefaultExtension;

    public ImageStoreService(IConfiguration configuration)
    {
        _baseImagePath = configuration["ImageStore:BasePath"];
        _imageDefaultExtension = configuration["ImageStore:DefaultExtension"];
    }

    public async Task<string> StoreImageAsync(string base64StringImage, string subFolderName = "", CancellationToken cancellationToken = default)
    {
        var imagePath = Path.Combine(_baseImagePath, subFolderName, $"{Guid.NewGuid()}{_imageDefaultExtension}");

        var bytes = Convert.FromBase64String(base64StringImage.Replace("data:image/png;base64,", ""));

        await File.WriteAllBytesAsync(imagePath, bytes, cancellationToken);
        return imagePath;
    }

    public async Task<string> GetBase64StringImageFromUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        if (url is null)
            return null;

        var bytes = await File.ReadAllBytesAsync(url, cancellationToken);

        return Convert.ToBase64String(bytes);
    }
}
