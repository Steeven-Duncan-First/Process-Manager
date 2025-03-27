using System;
using System.Collections.Generic;

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

    public class User
    {
        public string Id { get; set; }
        public string EmployeeCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Service { get; set; }
        public string Function { get; set; }
        public int MaxAccessLevel { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
    }

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

    public class AccessLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProcedureId { get; set; }
        public DateTime AccessDate { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
    }
} 