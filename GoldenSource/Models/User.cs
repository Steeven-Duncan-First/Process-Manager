using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoldenSource.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required]
        public string EmployeeCode { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Department { get; set; }
        public string Service { get; set; }
        public string Function { get; set; }

        public AccessLevel MaxAccessLevel { get; set; }

        public List<string> Roles { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLoginDate { get; set; }

        public List<ProcedureAccessLog> AccessLogs { get; set; }
    }
} 