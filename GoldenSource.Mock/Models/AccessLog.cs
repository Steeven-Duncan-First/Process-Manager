using System;

namespace GoldenSource.Mock.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime AccessDate { get; set; }
        public string IpAddress { get; set; }
    }
} 