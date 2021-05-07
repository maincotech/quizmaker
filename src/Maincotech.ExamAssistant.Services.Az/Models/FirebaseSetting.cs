using System.Text.Json.Serialization;

namespace Maincotech.ExamAssistant.Services.Models
{
    public class FirebaseSetting
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userid")]
        public string UserId { get; set; }

        [JsonPropertyName("projectid")]
        public string ProjectId { get; set; }

        [JsonPropertyName("jsoncredentials")]
        public string JsonCredentials { get; set; }
    }
}