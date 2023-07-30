using System.Text.RegularExpressions;
using Imagine.Core.Configurations;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Imagine.Infrastructure.Persistence;

public class ArtStorage : IArtStorage
{
    private readonly AppSettings _appSettings;

    public ArtStorage(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public async Task<Art> StoreArtAsync(SdResponse response, Art art)
    {
        var storagePath = Path.Join(_appSettings.ExecutionDirectory, _appSettings.StorageDir,
            art.Type.ToString(), art.User.UserName, art.TaskId.ToString());

        foreach (var image in response.ImageList)
        {
            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(storagePath, fileName);
            var result = await SaveBase64ImageAsync(image, filePath);
            if (!result) throw new Exception($"Failed to save image: {filePath}");

            art.Urls.Add(filePath);
        }

        return art;
    }

    private async Task<bool> SaveBase64ImageAsync(string base64Image, string filePath)
    {
        try
        {
            var binData = Convert.FromBase64String(base64Image);

            var file = new FileInfo(filePath);
            file.Directory?.Create();

            await File.WriteAllBytesAsync(file.FullName, binData);
            return File.Exists(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
