using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenSourceMock.Models
{
    public static class MockData
    {
        public static List<Procedure> Procedures { get; } = new List<Procedure>
        {
            new Procedure
            {
                Id = 1,
                Title = "Ouverture de compte",
                DocumentPath = "/documents/ouverture-compte.pdf",
                AccessLevel = 1,
                Department = "Réseau",
                Service = "Accueil",
                Function = "Conseiller",
                Recommendations = "Vérifier l'identité du client",
                CreatedAt = DateTime.Now.AddDays(-30),
                UpdatedAt = DateTime.Now.AddDays(-5),
                IsArchived = false
            },
            new Procedure
            {
                Id = 2,
                Title = "Fermeture de compte",
                DocumentPath = "/documents/fermeture-compte.pdf",
                AccessLevel = 2,
                Department = "Réseau",
                Service = "Accueil",
                Function = "Conseiller",
                Recommendations = "Vérifier les soldes et les opérations en cours",
                CreatedAt = DateTime.Now.AddDays(-60),
                UpdatedAt = DateTime.Now.AddDays(-10),
                IsArchived = false
            }
        };

        public static List<User> Users { get; } = new List<User>
        {
            new User
            {
                Id = "1",
                UserName = "jdupont",
                FirstName = "Jean",
                LastName = "Dupont",
                Email = "jean.dupont@socgen.com",
                Department = "Réseau",
                Service = "Accueil",
                Function = "Conseiller",
                MaxAccessLevel = 2,
                Roles = string.Join(",", new List<string> { "Utilisateur", "Administrateur" }),
                IsActive = true
            },
            new User
            {
                Id = "2",
                UserName = "mlambert",
                FirstName = "Marie",
                LastName = "Lambert",
                Email = "marie.lambert@socgen.com",
                Department = "Réseau",
                Service = "Accueil",
                Function = "Conseiller",
                MaxAccessLevel = 1,
                Roles = "Utilisateur",
                IsActive = true
            }
        };

        public static List<ChatMessage> ChatMessages { get; } = new List<ChatMessage>
        {
            new ChatMessage
            {
                Id = 1,
                UserId = "1",
                Content = "Comment puis-je ouvrir un compte ?",
                IsUserMessage = true,
                Timestamp = DateTime.Now.AddMinutes(-5)
            },
            new ChatMessage
            {
                Id = 2,
                UserId = "1",
                Content = "Pour ouvrir un compte, suivez la procédure 'Ouverture de compte' qui se trouve dans la section Réseau > Accueil.",
                IsUserMessage = false,
                Timestamp = DateTime.Now.AddMinutes(-4)
            }
        };

        public static List<FAQ> FAQs { get; } = new List<FAQ>
        {
            new FAQ
            {
                Id = 1,
                Question = "Comment ouvrir un compte ?",
                Answer = "Pour ouvrir un compte, suivez la procédure 'Ouverture de compte' qui se trouve dans la section Réseau > Accueil.",
                Category = "Comptes",
                CreatedAt = DateTime.Now.AddDays(-30),
                UpdatedAt = DateTime.Now.AddDays(-5)
            },
            new FAQ
            {
                Id = 2,
                Question = "Comment fermer un compte ?",
                Answer = "Pour fermer un compte, suivez la procédure 'Fermeture de compte' qui se trouve dans la section Réseau > Accueil.",
                Category = "Comptes",
                CreatedAt = DateTime.Now.AddDays(-30),
                UpdatedAt = DateTime.Now.AddDays(-5)
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