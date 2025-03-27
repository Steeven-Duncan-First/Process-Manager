using System;

namespace GoldenSource.Mock.Models
{
    public class FrequentlyAskedQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Language { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 