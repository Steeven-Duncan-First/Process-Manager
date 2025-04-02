using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public interface IMockChatbotService
    {
        Task<ChatMessage> ProcessMessageAsync(string message, string userId);
        Task<IEnumerable<FAQ>> GetFAQsAsync(string language);
        Task<Feedback> AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment);
    }
} 