using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenSourceMock.Services;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IMockChatbotService _chatbotService;
        private readonly IMockProcedureService _procedureService;

        public ChatbotController(IMockChatbotService chatbotService, IMockProcedureService procedureService)
        {
            _chatbotService = chatbotService;
            _procedureService = procedureService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            // Pour la démo, on utilise un ID utilisateur fixe
            var userId = "1";
            var response = await _chatbotService.ProcessMessageAsync(message.Message, userId);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            // Pour la démo, on utilise un ID utilisateur fixe
            var userId = "1";
            var result = await _chatbotService.AddFeedbackAsync(feedback.ChatMessageId, userId, feedback.IsHelpful, feedback.Comment);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFAQs(string language = "fr")
        {
            var faqs = await _chatbotService.GetFAQsAsync(language);
            return Json(faqs);
        }
    }
} 