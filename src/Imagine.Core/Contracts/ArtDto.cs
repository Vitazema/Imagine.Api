using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public record ArtDto : IValidatableObject
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public List<string> Urls { get; set; }
    public int Rating { get; set; }
    [JsonPropertyName("favorite")]
    public bool IsFavorite { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string User { get; set; }
    [Required] public ArtType ArtType { get; set; }
    [Required]
    [JsonPropertyName("parameters")]
    public JsonNode ArtSetting { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!IsValidJson(ArtSetting.ToJsonString()))
        {
            yield return new ValidationResult("artSetting Json object is not valid");
        }

        var prompt = ArtSetting["prompt"];

        if (prompt is null)
            yield return new ValidationResult("prompt is required");

        else if (string.IsNullOrEmpty(prompt.ToString()) ||
                 prompt.ToString().Length < 3 ||
                 prompt.ToString().Length > 500)
        {
            yield return new ValidationResult("prompt cannot be empty and between 3 and 500 characters");
        }
    }

    private static bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JsonSerializer.Deserialize(strInput, typeof(object));
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
