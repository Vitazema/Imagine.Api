namespace Imagine.Core.Configurations;

public record StableDiffusionWorker
{
    public int Id { get; set; }
    public string Address { get; set; }
    public bool Enabled { get; set; }
    public int QueueCapacity { get; set; }
}