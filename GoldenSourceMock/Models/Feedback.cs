using System;

namespace GoldenSourceMock.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChatMessageId { get; set; }
        public bool IsHelpful { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 