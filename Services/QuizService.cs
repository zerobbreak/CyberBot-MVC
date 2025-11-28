using ChatBot.Models;
using MongoDB.Driver;

namespace ChatBot.Services
{
    public class QuizService
    {
        private readonly MongoDbService _mongoDbService;
        private readonly List<QuizQuestion> _questions;

        public QuizService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion { Id = "1", QuestionText = "What is the recommended minimum password length?", Options = new List<string> { "6 chars", "8 chars", "12 chars", "16 chars" }, CorrectOptionIndex = 2, Explanation = "12 characters is the modern standard for security." },
                new QuizQuestion { Id = "2", QuestionText = "What does Phishing usually involve?", Options = new List<string> { "Fishing for fish", "Deceptive emails", "Virus download", "Hacking wifi" }, CorrectOptionIndex = 1, Explanation = "Phishing uses deceptive emails to steal data." },
                new QuizQuestion { Id = "3", QuestionText = "Which protocol is secure for browsing?", Options = new List<string> { "HTTP", "FTP", "HTTPS", "SMTP" }, CorrectOptionIndex = 2, Explanation = "HTTPS encrypts your traffic." },
                new QuizQuestion { Id = "4", QuestionText = "What is 2FA?", Options = new List<string> { "2 Fast Apps", "Two-Factor Authentication", "To For All", "None" }, CorrectOptionIndex = 1, Explanation = "2FA adds a second layer of security." },
                new QuizQuestion { Id = "5", QuestionText = "What is a firewall?", Options = new List<string> { "A wall of fire", "Network security device", "Antivirus", "Browser" }, CorrectOptionIndex = 1, Explanation = "Firewalls monitor and control network traffic." },
                new QuizQuestion { Id = "6", QuestionText = "Should you reuse passwords?", Options = new List<string> { "Yes", "No", "Only for banks", "Sometimes" }, CorrectOptionIndex = 1, Explanation = "Never reuse passwords to prevent credential stuffing." },
                new QuizQuestion { Id = "7", QuestionText = "What is social engineering?", Options = new List<string> { "Building bridges", "Manipulating people", "Coding", "Social media" }, CorrectOptionIndex = 1, Explanation = "It relies on human error rather than technical hacking." },
                new QuizQuestion { Id = "8", QuestionText = "What is ransomware?", Options = new List<string> { "Free software", "Malware that encrypts files", "Antivirus", "A game" }, CorrectOptionIndex = 1, Explanation = "Ransomware demands payment to decrypt files." },
                new QuizQuestion { Id = "9", QuestionText = "How often should you update software?", Options = new List<string> { "Never", "Annually", "As soon as possible", "When it breaks" }, CorrectOptionIndex = 2, Explanation = "Updates often contain critical security patches." },
                new QuizQuestion { Id = "10", QuestionText = "Is public Wi-Fi safe?", Options = new List<string> { "Yes, always", "No, generally unsafe", "Only at airports", "Yes, if password protected" }, CorrectOptionIndex = 1, Explanation = "Public Wi-Fi is often unencrypted and risky." }
            };
        }

        public List<QuizQuestion> GetQuestions()
        {
            return _questions;
        }

        public QuizQuestion? GetQuestion(string id)
        {
            return _questions.FirstOrDefault(q => q.Id == id);
        }

        public async Task SaveAttemptAsync(QuizAttempt attempt)
        {
            var collection = _mongoDbService.GetCollection<QuizAttempt>("quizAttempts");
            await collection.InsertOneAsync(attempt);
        }
    }
}
