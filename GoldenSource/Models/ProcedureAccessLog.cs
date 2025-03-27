using System;

namespace GoldenSource.Models
{
    public class ProcedureAccessLog
    {
        public int Id { get; set; }

        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public DateTime AccessDate { get; set; }

        public string Action { get; set; } // View, Download, etc.

        public string IpAddress { get; set; }

        public string UserAgent { get; set; }
    }
} 