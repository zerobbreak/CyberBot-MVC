using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace ChatBot.Controllers
{
    public class LogController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public LogController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var collection = _mongoDbService.GetCollection<ActivityLog>("logs");
            var logs = await collection.Find(l => l.UserId == userId)
                                       .SortByDescending(l => l.Timestamp)
                                       .Limit(100)
                                       .ToListAsync();
            return View(logs);
        }
    }
}
