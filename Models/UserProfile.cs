using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Models
{
    public class UserProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = "User";

        public string? FavoriteTopic { get; set; }
    }
}
