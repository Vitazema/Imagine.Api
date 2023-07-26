using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Imagine.Core.Contracts;

public record SdProgressResponse
{
    [JsonPropertyName("progress")]
    public double Progress { get; set; }
    [JsonPropertyName("eta_relative")]
    public double EtaRelative { get; set; }
    public JsonNode State { get; set; }
    [JsonPropertyName("current_image")]
    public string CurrentImage { get; set; }
    [JsonPropertyName("textinfo")]
    public string TextInfo { get; set; }
}
