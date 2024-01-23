using Imagine.Core.Entities;

namespace Imagine.Core.Interfaces;

public interface IAttachmentRepository
{
    Task<Attachment> GetAttachmentAsync(string id);
    Task<Attachment> UpsertAttachmentAsync(Attachment attachment);
    Task<bool> DeleteAttachmentAsync(string id);
}
