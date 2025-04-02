using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenSourceMock.Models
{
    public static class MockData
    {
        public static List<Procedure> Procedures = new List<Procedure>
        {
            new Procedure
            {
                Id = 1,
                Title = "Procédure d'ouverture de compte",
                DocumentPath = "/documents/ouverture-compte.pdf",
                AccessLevel = 1,
                Department = "RBC",
                AffectedServices = "Service Client, Back Office",
                Recommendations = "Vérifier l'identité du client",
                CreationDate = DateTime.Now.AddDays(-30),
                ExpirationDate = DateTime.Now.AddMonths(6),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            },
            new Procedure
            {
                Id = 2,
                Title = "Procédure de virement international",
                DocumentPath = "/documents/virement-international.pdf",
                AccessLevel = 2,
                Department = "RBC",
                AffectedServices = "Service Client, Back Office, Conformité",
                Recommendations = "Vérifier les sanctions internationales",
                CreationDate = DateTime.Now.AddDays(-60),
                ExpirationDate = DateTime.Now.AddMonths(3),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            }
        };

        public static List<User> Users = new List<User>
        {
            new User
            {
                Id = "1",
                EmployeeCode = "EMP001",
                UserName = "jean.dupont",
                FirstName = "Jean",
                LastName = "Dupont",
                Email = "jean.dupont@socgen.com",
                Department = "RBC",
                Service = "Service Client",
                Function = "Conseiller",
                MaxAccessLevel = 2,
                Roles = new List<string> { "User", "RBC" },
                IsActive = true,
                LastLoginDate = DateTime.Now
            }
        };

        public static List<ChatMessage> ChatMessages = new List<ChatMessage>
        {
            new ChatMessage
            {
                Id = 1,
                UserId = "1",
                Message = "Comment ouvrir un compte ?",
                Language = "fr",
                Intent = "search_procedure",
                Response = "Je peux vous aider à trouver la procédure d'ouverture de compte.",
                Timestamp = DateTime.Now.AddMinutes(-5),
                IsUserMessage = true,
                Confidence = 0.95,
                RelatedProcedureId = "1"
            }
        };

        public static List<FAQ> FAQs = new List<FAQ>
        {
            new FAQ
            {
                Id = 1,
                Question = "Comment accéder aux procédures ?",
                Answer = "Vous pouvez accéder aux procédures via le menu principal ou utiliser la barre de recherche.",
                Category = "Accès",
                Language = "fr",
                Order = 1,
                IsActive = true,
                LastUpdated = DateTime.Now
            }
        };

        public static List<Feedback> Feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                UserId = "1",
                ChatMessageId = 1,
                IsHelpful = true,
                Comment = "Très utile",
                Timestamp = DateTime.Now.AddMinutes(-4)
            }
        };
    }
} 