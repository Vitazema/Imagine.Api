using System.Text.Json;
using Imagine.Core.Configurations;
using Imagine.Core.Entities;
using Imagine.Core.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Imagine.Infrastructure.Persistence;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly IDatabase _database;
    private readonly TimeSpan _tempFilesExpirationInDays;

    public AttachmentRepository(IOptions<AppSettings> appSettings,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _tempFilesExpirationInDays = TimeSpan.FromMinutes(appSettings.Value.TempFilesExpirationInDays);
    }

    public async Task<Attachment> GetAttachmentAsync(string id)
    {
        var data = await _database.StringGetAsync(id);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Attachment>(data);
    }

    public async Task<Attachment> UpsertAttachmentAsync(Attachment attachment)
    {
        var created = await _database.StringSetAsync(attachment.Id.ToString(),
            JsonSerializer.Serialize(attachment), _tempFilesExpirationInDays);
        var newAttachment = await _database.StringGetAsync(attachment.Id.ToString());
        return !created ? null : JsonSerializer.Deserialize<Attachment>(newAttachment);
    }

    public async Task<bool> DeleteAttachmentAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
}
