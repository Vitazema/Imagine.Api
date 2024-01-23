namespace Imagine.Core.Entities;

public class Attachment : BaseEntity
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string Content { get; set; }
    public string FilePath { get; set; }
    public string FileExtension { get; set; }
    public string FilePreviewPath { get; set; }
}
