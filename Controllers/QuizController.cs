using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Question(int index = 0, int score = 0)
        {
            var questions = _quizService.GetQuestions();
            if (index >= questions.Count)
            {
                return RedirectToAction("Results", new { score, total = questions.Count });
            }

            var question = questions[index];
            ViewBag.Index = index;
            ViewBag.Score = score;
            ViewBag.Total = questions.Count;
            return View(question);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(int index, int score, int selectedOption)
        {
            var questions = _quizService.GetQuestions();
            var question = questions[index];
            
            if (selectedOption == question.CorrectOptionIndex)
            {
                score++;
                TempData["Feedback"] = "Correct!";
            }
            else
            {
                TempData["Feedback"] = $"Incorrect. {question.Explanation}";
            }

            return RedirectToAction("Question", new { index = index + 1, score });
        }

        public async Task<IActionResult> Results(int score, int total)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var attempt = new QuizAttempt
            {
                Score = score,
                TotalQuestions = total,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };
            await _quizService.SaveAttemptAsync(attempt);

            ViewBag.Score = score;
            ViewBag.Total = total;
            return View();
        }
    }
}
