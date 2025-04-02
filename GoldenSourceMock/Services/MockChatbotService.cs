using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public class MockChatbotService : IMockChatbotService
    {
        private readonly IMockProcedureService _procedureService;

        public MockChatbotService(IMockProcedureService procedureService)
        {
            _procedureService = procedureService;
        }

        public async Task<ChatMessage> ProcessMessageAsync(string message, string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(1000);

            // Détecter la langue (simplifié)
            var language = message.Contains("?") ? "fr" : "en";

            // Détecter l'intention (simplifié)
            var intent = "unknown";
            if (message.ToLower().Contains("procédure") || message.ToLower().Contains("procedure"))
                intent = "search_procedure";
            else if (message.ToLower().Contains("aide") || message.ToLower().Contains("help"))
                intent = "help";

            // Générer une réponse
            string response;
            string relatedProcedureId = null;

            switch (intent)
            {
                case "search_procedure":
                    var procedures = await _procedureService.SearchProceduresAsync(message, userId);
                    var procedure = procedures.FirstOrDefault();
                    if (procedure != null)
                    {
                        response = $"J'ai trouvé la procédure : {procedure.Title}";
                        relatedProcedureId = procedure.Id.ToString();
                    }
                    else
                    {
                        response = "Je n'ai pas trouvé de procédure correspondant à votre demande.";
                    }
                    break;

                case "help":
                    response = "Je peux vous aider à trouver des procédures. Que cherchez-vous ?";
                    break;

                default:
                    response = "Je ne comprends pas votre demande. Pouvez-vous la reformuler ?";
                    break;
            }

            var chatMessage = new ChatMessage
            {
                Id = MockData.ChatMessages.Count + 1,
                UserId = userId,
                Message = message,
                Language = language,
                Intent = intent,
                Response = response,
                Timestamp = DateTime.Now,
                IsUserMessage = true,
                Confidence = 0.9,
                RelatedProcedureId = relatedProcedureId
            };

            MockData.ChatMessages.Add(chatMessage);
            return chatMessage;
        }

        public async Task<IEnumerable<FAQ>> GetFAQsAsync(string language)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.FAQs
                .Where(f => f.Language == language && f.IsActive)
                .OrderBy(f => f.Order)
                .ToList();
        }

        public async Task<Feedback> AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment)
        {
            // Simuler un délai réseau
            await Task.Delay(200);

            var feedback = new Feedback
            {
                Id = MockData.Feedbacks.Count + 1,
                UserId = userId,
                ChatMessageId = chatMessageId,
                IsHelpful = isHelpful,
                Comment = comment,
                Timestamp = DateTime.Now
            };

            MockData.Feedbacks.Add(feedback);
            return feedback;
        }
    }
} 