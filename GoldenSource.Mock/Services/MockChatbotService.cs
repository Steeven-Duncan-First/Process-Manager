using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSource.Mock.Models;

namespace GoldenSource.Mock.Services
{
    public interface IMockChatbotService
    {
        Task<ChatMessage> ProcessMessageAsync(string userId, string message);
        Task<List<ChatMessage>> GetChatHistoryAsync(string userId);
        Task<List<FrequentlyAskedQuestion>> GetFrequentlyAskedQuestionsAsync(string language);
        Task AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment);
    }

    public class MockChatbotService : IMockChatbotService
    {
        private readonly IMockProcedureService _procedureService;
        private readonly Dictionary<string, string> _searchPatterns;
        private readonly Dictionary<string, string> _responses;

        public MockChatbotService(IMockProcedureService procedureService)
        {
            _procedureService = procedureService;
            _searchPatterns = InitializeSearchPatterns();
            _responses = InitializeResponses();
        }

        private Dictionary<string, string> InitializeSearchPatterns()
        {
            return new Dictionary<string, string>
            {
                { "recherche", @"(chercher|trouver|rechercher|où est|comment trouver).*procédure" },
                { "aide", @"(aide|help|assistance|comment faire)" }
            };
        }

        private Dictionary<string, string> InitializeResponses()
        {
            return new Dictionary<string, string>
            {
                { "recherche", "Je peux vous aider à trouver la procédure. Quel est le titre ou le sujet que vous recherchez ?" },
                { "aide", "Je peux vous aider à rechercher des procédures. Voici quelques questions fréquentes :\n" }
            };
        }

        public async Task<ChatMessage> ProcessMessageAsync(string userId, string message)
        {
            // Simuler un délai réseau
            await Task.Delay(1000);

            var language = DetectLanguage(message);
            var intent = DetectIntent(message, language);
            
            string response;
            string relatedProcedureId = null;

            if (intent == "recherche")
            {
                var procedures = await _procedureService.SearchProceduresAsync(message, userId);
                if (procedures.Any())
                {
                    response = GenerateSearchResponse(procedures, language);
                    relatedProcedureId = procedures.First().Id.ToString();
                }
                else
                {
                    response = language == "fr" 
                        ? "Je n'ai trouvé aucune procédure correspondant à votre recherche. Essayez avec d'autres mots-clés."
                        : "I couldn't find any procedures matching your search. Try with different keywords.";
                }
            }
            else
            {
                var faqs = await GetFrequentlyAskedQuestionsAsync(language);
                response = _responses[intent] + string.Join("\n", faqs.Select(f => $"- {f.Question}"));
            }

            var chatMessage = new ChatMessage
            {
                Id = MockData.ChatMessages.Count + 1,
                UserId = userId,
                Message = message,
                Language = language,
                Intent = intent,
                Response = response,
                Timestamp = DateTime.UtcNow,
                IsUserMessage = true,
                Confidence = 0.8,
                RelatedProcedureId = relatedProcedureId
            };

            MockData.ChatMessages.Add(chatMessage);
            return chatMessage;
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public async Task<List<FrequentlyAskedQuestion>> GetFrequentlyAskedQuestionsAsync(string language)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            return MockData.FAQs
                .Where(f => f.Language == language && f.IsActive)
                .OrderByDescending(f => f.UsageCount)
                .Take(5)
                .ToList();
        }

        public async Task AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment)
        {
            // Simuler un délai réseau
            await Task.Delay(500);

            var chatMessage = MockData.ChatMessages.FirstOrDefault(m => m.Id == chatMessageId);
            if (chatMessage != null && !string.IsNullOrEmpty(chatMessage.RelatedProcedureId))
            {
                var faq = MockData.FAQs
                    .FirstOrDefault(f => f.Question == chatMessage.Message);
                if (faq != null)
                {
                    faq.UsageCount++;
                    faq.LastUpdated = DateTime.UtcNow;
                }
            }
        }

        private string DetectLanguage(string message)
        {
            if (message.Any(c => "éèêëàâäôöûüçîï".Contains(c)))
                return "fr";
            return "en";
        }

        private string DetectIntent(string message, string language)
        {
            message = message.ToLower();
            foreach (var pattern in _searchPatterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(message, pattern.Value, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    return pattern.Key;
                }
            }
            return language == "fr" ? "aide" : "help";
        }

        private string GenerateSearchResponse(IEnumerable<Procedure> procedures, string language)
        {
            if (language == "fr")
            {
                return $"J'ai trouvé {procedures.Count()} procédure(s) :\n" +
                       string.Join("\n", procedures.Select(p => 
                           $"- {p.Title} (Niveau d'accès : {p.AccessLevel})"));
            }
            return $"I found {procedures.Count()} procedure(s):\n" +
                   string.Join("\n", procedures.Select(p => 
                       $"- {p.Title} (Access Level: {p.AccessLevel})"));
        }
    }
} 