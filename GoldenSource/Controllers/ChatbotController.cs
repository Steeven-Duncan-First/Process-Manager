using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldenSource.Services;
using GoldenSource.Models.Chatbot;

namespace GoldenSource.Controllers
{
    [Authorize]
    public class ChatbotController : Controller
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var chatHistory = await _chatbotService.GetChatHistoryAsync(userId);
            return View(chatHistory);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessMessage([FromBody] ChatRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var response = await _chatbotService.ProcessMessageAsync(userId, request.Message);
            return Json(new { response = response.Response });
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            await _chatbotService.AddFeedbackAsync(request.ChatMessageId, userId, request.IsHelpful, request.Comment);
            return Ok();
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }

    public class FeedbackRequest
    {
        public int ChatMessageId { get; set; }
        public bool IsHelpful { get; set; }
        public string Comment { get; set; }
    }
} 