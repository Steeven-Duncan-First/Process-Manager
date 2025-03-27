using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoldenSource.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le document est obligatoire")]
        public string DocumentPath { get; set; }

        [Required(ErrorMessage = "Le niveau d'accès est obligatoire")]
        public AccessLevel AccessLevel { get; set; }

        public string Department { get; set; }

        public List<string> AffectedServices { get; set; }

        public string Recommendations { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsArchived { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public List<ProcedureAccessLog> AccessLogs { get; set; }
    }

    public enum AccessLevel
    {
        C1,
        C2,
        C3
    }
} 