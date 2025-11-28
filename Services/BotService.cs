using ChatBot.Models;
using MongoDB.Driver;

namespace ChatBot.Services
{
    public class BotService
    {
        private readonly NlpService _nlpService;
        private readonly MongoDbService _mongoDbService;

        public BotService(NlpService nlpService, MongoDbService mongoDbService)
        {
            _nlpService = nlpService;
            _mongoDbService = mongoDbService;
        }

        public async Task<BotMessage> ProcessMessageAsync(string userId, string userText, int clientOffset)
        {
            // 1. Analyze Input
            var topic = _nlpService.RecognizeTopic(userText);
            var sentiment = _nlpService.DetectSentiment(userText);
            var (command, commandData) = _nlpService.ParseCommand(userText);

            // 2. Determine Response
            string responseText = GenerateResponse(topic, sentiment, command, commandData);

            // SPECIAL HANDLING: Execute Commands
            if (command == "add_task" && !string.IsNullOrEmpty(commandData))
            {
                // Calculate User's Local Time
                // clientOffset is in minutes (UTC - Local). So Local = UTC - offset
                // Example: UTC is 12:00. Offset is -120 (GMT+2). Local = 12:00 - (-120m) = 14:00.
                var userLocalTime = DateTime.UtcNow.AddMinutes(-clientOffset);
                
                // Default reminder: Tomorrow at 9 AM user time
                var reminderLocal = userLocalTime.AddDays(1).Date.AddHours(9);
                
                // Convert back to UTC for storage
                // UTC = Local + offset
                var reminderUtc = reminderLocal.AddMinutes(clientOffset);

                var taskCollection = _mongoDbService.GetCollection<TaskItem>("tasks");
                var newTask = new TaskItem
                {
                    Description = commandData,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    ReminderTime = reminderUtc,
                    UserId = userId
                };
                await taskCollection.InsertOneAsync(newTask);
                responseText = $"I've added '{commandData}' to your task list. Set for {reminderLocal:g} (your time).";
            }

            // 3. Create Message Objects
            var userMessage = new BotMessage
            {
                Text = userText,
                Sender = "User",
                Topic = topic,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };

            var botMessage = new BotMessage
            {
                Text = responseText,
                Sender = "Bot",
                Topic = topic,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };

            // 4. Persist to MongoDB
            var msgCollection = _mongoDbService.GetCollection<BotMessage>("messages");
            await msgCollection.InsertOneAsync(userMessage);
            await msgCollection.InsertOneAsync(botMessage);

            // 5. Log Activity
            var log = new ActivityLog
            {
                UserMessage = userText,
                RecognizedIntent = topic,
                BotResponseType = sentiment,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };
            var logCollection = _mongoDbService.GetCollection<ActivityLog>("logs");
            await logCollection.InsertOneAsync(log);

            return botMessage;
        }

        private string GenerateResponse(string topic, string sentiment, string? command, string? commandData)
        {
            if (command == "start_quiz")
            {
                return "Starting quiz... Navigate to the Quiz tab to begin!";
            }



            // add_task is handled in ProcessMessageAsync to perform the DB write


            if (command == "view_tasks")
            {
                return "You can view all your pending security tasks in the 'Tasks' tab. Would you like me to take you there? (Type '/tasks')";
            }

            string baseResponse = "";

            switch (topic)
            {
                case "Password Safety":
                    baseResponse = "Strong passwords are your first line of defense. Use at least 12 characters, mixing letters, numbers, and symbols.";
                    break;
                case "Phishing / Scams":
                    baseResponse = "Be wary of unexpected emails asking for personal info. Check the sender's address carefully.";
                    break;
                case "Privacy":
                    baseResponse = "Your data is valuable. Review app permissions and browser privacy settings regularly.";
                    break;
                case "Safe Browsing":
                    baseResponse = "Look for the padlock icon (HTTPS) and avoid clicking suspicious pop-ups.";
                    break;
                case "Firewalls":
                    baseResponse = "Firewalls block unauthorized access. Ensure your OS firewall is always enabled.";
                    break;
                case "Malware":
                    baseResponse = "Malware includes viruses and ransomware. Run a full system scan immediately if you suspect an infection.";
                    break;
                default:
                    baseResponse = "I can help you with cybersecurity topics like passwords, phishing, privacy, and more. What would you like to know?";
                    break;
            }

            // Adjust for sentiment
            if (sentiment == "Worried")
            {
                return $"It's okay to be concerned, but taking action helps. {baseResponse} We can secure this together.";
            }
            else if (sentiment == "Frustrated")
            {
                return $"I understand this can be annoying. Let's make it simple. {baseResponse}";
            }
            else if (sentiment == "Curious")
            {
                return $"{baseResponse} Would you like to dive deeper into the technical details?";
            }
            else if (sentiment == "Happy")
            {
                return $"That's great to hear! {baseResponse} Stay safe!";
            }

            return baseResponse;
        }
    }
}
