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

    public async Task<Art> StoreArtAsync(SdQueueTaskResult taskResult, Art art)
    {
        var fullPath = Path.Join(_appSettings.StorageDir, art.Type.ToString(), art.User.UserName,
            art.Id.ToString());

        foreach (var imageData in taskResult.Data)
        {
            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(fullPath, fileName);
            var result = await SaveBase64ImageAsync(imageData.Image, filePath);
            if (!result) throw new Exception($"Failed to save image: {filePath}");
            
            var relativeStoragePath = Path.Join(art.Type.ToString(), art.User.UserName,
            art.Id.ToString(), fileName);
            
            art.Urls.Add(relativeStoragePath);
        }

        return art;
    }

    private async Task<bool> SaveBase64ImageAsync(string base64Image, string filePath)
    {
        try
        {
            base64Image = Regex.Replace(base64Image, "^data:image/[a-z]+;base64,", "");
            
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
