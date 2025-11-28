using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Models
{
    public class BotMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Sender { get; set; } = "User"; // "User" or "Bot"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? Topic { get; set; }

        public string? UserId { get; set; }
    }
}
