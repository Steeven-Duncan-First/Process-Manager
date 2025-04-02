using System;

namespace GoldenSourceMock.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 