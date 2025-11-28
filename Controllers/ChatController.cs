using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace ChatBot.Controllers
{
    public class ChatController : Controller
    {
        private readonly BotService _botService;
        private readonly MongoDbService _mongoDbService;

        public ChatController(BotService botService, MongoDbService mongoDbService)
        {
            _botService = botService;
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            // Load recent messages for this user
            var collection = _mongoDbService.GetCollection<BotMessage>("messages");
            var messages = await collection.Find(m => m.UserId == userId)
                                           .SortByDescending(m => m.Timestamp)
                                           .Limit(50)
                                           .ToListAsync();
            messages.Reverse(); // Show oldest first in chat window
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string userMessage, int clientOffset)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(userMessage))
                return RedirectToAction("Index");

            // Navigation Commands
            var lowerMsg = userMessage.Trim().ToLower();
            if (lowerMsg == "/quiz" || lowerMsg == "open quiz" || lowerMsg == "start quiz")
            {
                return RedirectToAction("Index", "Quiz");
            }
            if (lowerMsg == "/tasks" || lowerMsg == "open tasks" || lowerMsg == "view tasks")
            {
                return RedirectToAction("Index", "Task");
            }
            if (lowerMsg == "/logs" || lowerMsg == "open logs" || lowerMsg == "view logs")
            {
                return RedirectToAction("Index", "Log");
            }
            if (lowerMsg == "/clear")
            {
                return RedirectToAction("Index");
            }

            await _botService.ProcessMessageAsync(userId, userMessage, clientOffset);

            return RedirectToAction("Index");
        }
    }
}
