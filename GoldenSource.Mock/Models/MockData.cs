using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenSource.Mock.Models
{
    public static class MockData
    {
        public static List<Procedure> Procedures = new List<Procedure>
        {
            new Procedure
            {
                Id = 1,
                Title = "Procédure d'ouverture de compte particulier",
                DocumentPath = "/documents/compte_particulier.pdf",
                AccessLevel = 1,
                Department = "Réseau",
                AffectedServices = "Accueil, Création de compte",
                Recommendations = "Vérifier l'identité du client",
                CreationDate = DateTime.Now.AddDays(-30),
                ExpirationDate = DateTime.Now.AddYears(1),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            },
            new Procedure
            {
                Id = 2,
                Title = "Procédure d'ouverture de compte professionnel",
                DocumentPath = "/documents/compte_professionnel.pdf",
                AccessLevel = 2,
                Department = "Réseau",
                AffectedServices = "Accueil, Création de compte",
                Recommendations = "Vérifier le Kbis",
                CreationDate = DateTime.Now.AddDays(-25),
                ExpirationDate = DateTime.Now.AddYears(1),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            },
            new Procedure
            {
                Id = 3,
                Title = "Procédure de virement SEPA",
                DocumentPath = "/documents/virement_sepa.pdf",
                AccessLevel = 1,
                Department = "Opérations",
                AffectedServices = "Virements, Opérations",
                Recommendations = "Vérifier les limites",
                CreationDate = DateTime.Now.AddDays(-20),
                ExpirationDate = DateTime.Now.AddYears(1),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            },
            new Procedure
            {
                Id = 4,
                Title = "Procédure de demande de carte",
                DocumentPath = "/documents/demande_carte.pdf",
                AccessLevel = 1,
                Department = "Cartes",
                AffectedServices = "Cartes, Accueil",
                Recommendations = "Vérifier le niveau de carte",
                CreationDate = DateTime.Now.AddDays(-15),
                ExpirationDate = DateTime.Now.AddYears(1),
                IsArchived = false,
                CreatedBy = "admin",
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.Now
            },
            new Procedure
            {
                Id = 5,
                Title = "Procédure de blocage de carte",
                DocumentPath = "/documents/blocage_carte.pdf",
                AccessLevel = 1,
                Department = "Cartes",
                AffectedServices = "Cartes, Service client",
                Recommendations = "Vérifier l'urgence",
                CreationDate = DateTime.Now.AddDays(-10),
                ExpirationDate = DateTime.Now.AddYears(1),
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
                Id = "user1",
                EmployeeCode = "EMP001",
                UserName = "jean.dupont",
                FirstName = "Jean",
                LastName = "Dupont",
                Email = "jean.dupont@socgen.com",
                Department = "Réseau",
                Service = "Accueil",
                Function = "Conseiller",
                MaxAccessLevel = 2,
                Roles = "Conseiller, Créateur",
                IsActive = true,
                LastLoginDate = DateTime.Now
            },
            new User
            {
                Id = "user2",
                EmployeeCode = "EMP002",
                UserName = "marie.martin",
                FirstName = "Marie",
                LastName = "Martin",
                Email = "marie.martin@socgen.com",
                Department = "Opérations",
                Service = "Virements",
                Function = "Opérateur",
                MaxAccessLevel = 1,
                Roles = "Opérateur",
                IsActive = true,
                LastLoginDate = DateTime.Now
            }
        };

        public static List<ChatMessage> ChatMessages = new List<ChatMessage>
        {
            new ChatMessage
            {
                Id = 1,
                UserId = "user1",
                Message = "Je cherche la procédure d'ouverture de compte",
                Language = "fr",
                Intent = "recherche",
                Response = "J'ai trouvé 2 procédures concernant l'ouverture de compte :\n- Procédure d'ouverture de compte particulier (Niveau d'accès : 1)\n- Procédure d'ouverture de compte professionnel (Niveau d'accès : 2)",
                Timestamp = DateTime.Now.AddHours(-2),
                IsUserMessage = false,
                Confidence = 0.9,
                RelatedProcedureId = "1"
            }
        };

        public static List<FrequentlyAskedQuestion> FAQs = new List<FrequentlyAskedQuestion>
        {
            new FrequentlyAskedQuestion
            {
                Id = 1,
                Question = "Comment ouvrir un compte particulier ?",
                Answer = "La procédure d'ouverture de compte particulier se trouve dans la section 'Réseau'. Vérifiez que vous avez le niveau d'accès requis.",
                Language = "fr",
                UsageCount = 150,
                IsActive = true,
                LastUpdated = DateTime.Now
            },
            new FrequentlyAskedQuestion
            {
                Id = 2,
                Question = "Comment effectuer un virement SEPA ?",
                Answer = "La procédure de virement SEPA est disponible dans la section 'Opérations'. Assurez-vous de vérifier les limites avant de procéder.",
                Language = "fr",
                UsageCount = 120,
                IsActive = true,
                LastUpdated = DateTime.Now
            }
        };
    }
} 