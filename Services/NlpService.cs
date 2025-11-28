using System.Text.RegularExpressions;

namespace ChatBot.Services
{
    public class NlpService
    {
        // Topic Definitions with weighted keywords
        private readonly Dictionary<string, List<string>> _topicKeywords = new()
        {
            { "Password Safety", new List<string> { "password", "passcode", "login", "credential", "auth", "2fa", "mfa" } },
            { "Phishing / Scams", new List<string> { "phishing", "scam", "fraud", "fake email", "suspicious link", "prince", "urgent" } },
            { "Privacy", new List<string> { "privacy", "data", "tracking", "cookies", "gdpr", "personal info", "spyware" } },
            { "Firewalls", new List<string> { "firewall", "block", "port", "network security", "traffic" } },
            { "Safe Browsing", new List<string> { "https", "ssl", "lock icon", "certificate", "url", "website" } },
            { "Malware", new List<string> { "virus", "trojan", "ransomware", "infected", "slow pc", "popup" } }
        };

        // Sentiment Definitions
        private readonly HashSet<string> _negativeWords = new() { "worried", "scared", "afraid", "help", "hacked", "stolen", "lost", "danger", "panic", "urgent" };
        private readonly HashSet<string> _curiousWords = new() { "how", "why", "what", "explain", "details", "more", "learn", "teach" };
        private readonly HashSet<string> _frustratedWords = new() { "stupid", "slow", "broken", "angry", "hate", "annoying", "fail", "error" };
        private readonly HashSet<string> _positiveWords = new() { "thanks", "good", "great", "safe", "secure", "fixed", "solved" };

        public string RecognizeTopic(string input)
        {
            var lowerInput = input.ToLower();
            string bestTopic = "General";
            double highestScore = 0;

            foreach (var topic in _topicKeywords)
            {
                double topicScore = 0;
                foreach (var keyword in topic.Value)
                {
                    // Exact match bonus
                    if (lowerInput.Contains(keyword))
                    {
                        topicScore += 1.0;
                    }
                    else
                    {
                        // Fuzzy match check (Levenshtein)
                        var words = lowerInput.Split(' ');
                        foreach (var word in words)
                        {
                            // Only check if lengths are somewhat similar to avoid waste
                            if (Math.Abs(word.Length - keyword.Length) <= 2) 
                            {
                                int distance = ComputeLevenshteinDistance(word, keyword);
                                // Allow 1 edit for short words, 2 for longer
                                int maxEdits = keyword.Length > 4 ? 2 : 1;
                                if (distance <= maxEdits)
                                {
                                    topicScore += 0.8;
                                }
                            }
                        }
                    }
                }

                if (topicScore > highestScore)
                {
                    highestScore = topicScore;
                    bestTopic = topic.Key;
                }
            }

            return highestScore > 0 ? bestTopic : "General";
        }

        public string DetectSentiment(string input)
        {
            var lowerInput = input.ToLower();
            var words = lowerInput.Split(new[] { ' ', '.', ',', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);

            // Check for negation (e.g., "not worried")
            bool hasNegation = words.Any(w => w == "not" || w == "dont" || w == "don't" || w == "never");

            int negativeScore = words.Count(w => _negativeWords.Contains(w));
            int curiousScore = words.Count(w => _curiousWords.Contains(w));
            int frustratedScore = words.Count(w => _frustratedWords.Contains(w));
            int positiveScore = words.Count(w => _positiveWords.Contains(w));

            if (hasNegation)
            {
                if (negativeScore > 0) return "Neutral";
                if (frustratedScore > 0) return "Neutral";
            }

            if (frustratedScore > 0) return "Frustrated";
            if (negativeScore > 0) return "Worried";
            if (curiousScore > 0) return "Curious";
            if (positiveScore > 0) return "Happy";

            return "Neutral";
        }

        public (string? Command, string? Data) ParseCommand(string input)
        {
            var lowerInput = input.ToLower().Trim();

            // Add Task
            var addTaskRegex = new Regex(@"(?:add|create|new|remind me to)\s+(?:task\s+)?(.+)", RegexOptions.IgnoreCase);
            var addTaskMatch = addTaskRegex.Match(lowerInput);
            if (addTaskMatch.Success)
            {
                return ("add_task", addTaskMatch.Groups[1].Value.Trim());
            }

            // Quiz
            if (lowerInput.Contains("quiz") || lowerInput.Contains("test my knowledge"))
            {
                return ("start_quiz", null);
            }

            // Navigation
            if (lowerInput.Contains("show tasks") || lowerInput.Contains("view tasks") || lowerInput.Contains("my list"))
            {
                return ("view_tasks", null);
            }

            return (null, null);
        }

        // Manual Levenshtein Implementation
        private static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
