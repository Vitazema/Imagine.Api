using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Entities;

public class Art : BaseEntity
{
    public string Title { get; set; }
    public List<string> Urls { get; set; } = new();
    public User User { get; set; }
    public string UserId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ArtType Type { get; set; }
    public int WorkerId { get; set; }
    public string ArtSetting { get; set; }
    public void SetArtSetting(string key, string value)
    {
        var artSetting = JsonNode.Parse(ArtSetting);
        if (artSetting == null) throw new ArgumentException($"ArtSetting is not a valid JSON {key}:{value}");
        artSetting[key] = value;
        ArtSetting = artSetting.ToJsonString();
    }
}
