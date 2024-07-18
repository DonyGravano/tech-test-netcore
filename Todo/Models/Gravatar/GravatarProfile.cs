using System.Text.Json.Serialization;

namespace Todo.Models.Gravatar
{
    public class GravatarProfile
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
    }
}
