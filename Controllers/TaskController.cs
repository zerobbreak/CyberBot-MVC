using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace ChatBot.Controllers
{
    public class TaskController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public TaskController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var collection = _mongoDbService.GetCollection<TaskItem>("tasks");
            var tasks = await collection.Find(t => t.UserId == userId).ToListAsync();
            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string description, DateTime? reminderTime)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            if (!string.IsNullOrWhiteSpace(description))
            {
                var collection = _mongoDbService.GetCollection<TaskItem>("tasks");
                var task = new TaskItem
                {
                    Description = description,
                    ReminderTime = reminderTime,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                };
                await collection.InsertOneAsync(task);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var collection = _mongoDbService.GetCollection<TaskItem>("tasks");
            await collection.DeleteOneAsync(t => t.Id == id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(string id, bool isCompleted)
        {
            var collection = _mongoDbService.GetCollection<TaskItem>("tasks");
            var update = Builders<TaskItem>.Update.Set(t => t.IsCompleted, isCompleted);
            await collection.UpdateOneAsync(t => t.Id == id, update);
            return RedirectToAction("Index");
        }
    }
}
