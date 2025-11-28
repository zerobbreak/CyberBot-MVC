using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Models
{
    public class ActivityLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string UserMessage { get; set; } = string.Empty;

        public string RecognizedIntent { get; set; } = string.Empty;

        public string BotResponseType { get; set; } = string.Empty;

        public string? UserId { get; set; }
    }
}
