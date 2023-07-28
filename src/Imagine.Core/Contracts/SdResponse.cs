using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdResponse
{
    [JsonPropertyName("images")]
    public List<string> ImageList { get; set; }

    [JsonPropertyName("parameters")]
    public Parameters Params { get; set; }

    [JsonPropertyName("info")]
    public string InfoJsonString { get; set; }
}

public record Parameters
{
    [JsonPropertyName("enable_hr")]
    public bool EnableHr { get; set; }
    [JsonPropertyName("denoising_strength")]
    public int DenoisingStrength { get; set; }
}