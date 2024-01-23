using Imagine.Core.Entities;
using Imagine.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

public class AttachmentController : BaseApiController
{
    private readonly IAttachmentRepository _attachmentRepository;

    public AttachmentController(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<Attachment>> GetAttachmentById(string id)
    {
        var attachment = await _attachmentRepository.GetAttachmentAsync(id);
        return Ok(attachment ?? new Attachment());
    }
    
    [HttpPost]
    public async Task<ActionResult<Attachment>> UpsertAttachment(Attachment attachment)
    {
        if (attachment.Id == Guid.Empty)
        {
            attachment.Id = Guid.NewGuid();
            attachment.CreatedAt = DateTime.UtcNow;
        }
        var updatedAttachment = await _attachmentRepository.UpsertAttachmentAsync(attachment);
        return Ok(updatedAttachment);
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteAttachment(string id)
    {
        var deleted = await _attachmentRepository.DeleteAttachmentAsync(id);
        return Ok(deleted);
    }
}
