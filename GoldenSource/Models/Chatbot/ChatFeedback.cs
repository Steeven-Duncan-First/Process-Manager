using System;

namespace GoldenSource.Models.Chatbot
{
    public class ChatFeedback
    {
        public int Id { get; set; }
        public int ChatMessageId { get; set; }
        public string UserId { get; set; }
        public bool IsHelpful { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string Language { get; set; }
    }
} 