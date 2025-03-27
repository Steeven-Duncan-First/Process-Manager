using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenSource.Mock.Services;

namespace GoldenSource.Mock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IMockChatbotService _chatbotService;

        public ChatbotController(IMockChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> ProcessMessage([FromBody] ChatMessageRequest request)
        {
            var response = await _chatbotService.ProcessMessageAsync(request.UserId, request.Message);
            return Ok(response);
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetChatHistory(string userId)
        {
            var history = await _chatbotService.GetChatHistoryAsync(userId);
            return Ok(history);
        }

        [HttpGet("faq/{language}")]
        public async Task<IActionResult> GetFrequentlyAskedQuestions(string language)
        {
            var faqs = await _chatbotService.GetFrequentlyAskedQuestionsAsync(language);
            return Ok(faqs);
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackRequest request)
        {
            await _chatbotService.AddFeedbackAsync(request.ChatMessageId, request.UserId, request.IsHelpful, request.Comment);
            return Ok();
        }
    }

    public class ChatMessageRequest
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }

    public class FeedbackRequest
    {
        public int ChatMessageId { get; set; }
        public string UserId { get; set; }
        public bool IsHelpful { get; set; }
        public string Comment { get; set; }
    }
} 