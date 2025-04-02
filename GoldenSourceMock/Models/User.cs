using System;

namespace GoldenSourceMock.Models
{
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
} 