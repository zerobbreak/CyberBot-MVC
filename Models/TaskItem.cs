using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Models
{
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReminderTime { get; set; }

        public string? UserId { get; set; }
    }
}
