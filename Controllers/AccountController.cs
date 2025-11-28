using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace ChatBot.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public AccountController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ViewBag.Error = "Username cannot be empty.";
                return View();
            }

            var collection = _mongoDbService.GetCollection<UserProfile>("users");
            var user = await collection.Find(u => u.Name == username).FirstOrDefaultAsync();

            if (user == null)
            {
                // Create new user
                user = new UserProfile
                {
                    Name = username
                };
                await collection.InsertOneAsync(user);
            }

            // Set Session
            HttpContext.Session.SetString("UserId", user.Id!);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Chat");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
