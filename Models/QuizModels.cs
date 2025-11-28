using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Models
{
    public class QuizQuestion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public List<string> Options { get; set; } = new List<string>();

        public int CorrectOptionIndex { get; set; }

        public string Explanation { get; set; } = string.Empty;
    }

    public class QuizAttempt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
    }
}
