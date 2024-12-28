using System.Text.Json.Serialization;

namespace Eci_website.Models
{
    public class ImageResponse
    {
        [JsonPropertyName("data")]
        public List<ImageData> Data { get; set; }
    }

    public class ImageData
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}