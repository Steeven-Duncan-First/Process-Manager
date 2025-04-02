using System;

namespace GoldenSourceMock.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
        public string Intent { get; set; }
        public string Response { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsUserMessage { get; set; }
        public double Confidence { get; set; }
        public string RelatedProcedureId { get; set; }
    }
} 