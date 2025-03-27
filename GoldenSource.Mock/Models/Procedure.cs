using System;

namespace GoldenSource.Mock.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DocumentPath { get; set; }
        public int AccessLevel { get; set; }
        public string Department { get; set; }
        public string AffectedServices { get; set; }
        public string Recommendations { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
} 