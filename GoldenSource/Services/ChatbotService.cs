using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using GoldenSource.Models.Chatbot;
using GoldenSource.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.Text;
using System.Text.RegularExpressions;

namespace GoldenSource.Services
{
    public interface IChatbotService
    {
        Task<ChatMessage> ProcessMessageAsync(string userId, string message);
        Task<List<ChatMessage>> GetChatHistoryAsync(string userId);
        Task<List<FrequentlyAskedQuestion>> GetFrequentlyAskedQuestionsAsync(string language);
        Task AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment);
    }

    public class ChatbotService : IChatbotService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IProcedureService _procedureService;
        private readonly Dictionary<string, string> _searchPatterns;
        private readonly Dictionary<string, string> _responses;

        public ChatbotService(
            ApplicationDbContext context, 
            IConfiguration configuration,
            IProcedureService procedureService)
        {
            _context = context;
            _configuration = configuration;
            _procedureService = procedureService;
            _searchPatterns = InitializeSearchPatterns();
            _responses = InitializeResponses();
        }

        private Dictionary<string, string> InitializeSearchPatterns()
        {
            return new Dictionary<string, string>
            {
                // Patterns en français
                { "recherche", @"(chercher|trouver|rechercher|où est|comment trouver).*procédure" },
                { "aide", @"(aide|help|assistance|comment faire)" },
                
                // Patterns en anglais
                { "search", @"(search|find|look for|where is|how to find).*procedure" },
                { "help", @"(help|assistance|how to|guide)" }
            };
        }

        private Dictionary<string, string> InitializeResponses()
        {
            return new Dictionary<string, string>
            {
                // Réponses en français
                { "recherche", "Je peux vous aider à trouver la procédure. Quel est le titre ou le sujet que vous recherchez ?" },
                { "aide", "Je peux vous aider à rechercher des procédures. Voici quelques questions fréquentes :\n" },
                
                // Réponses en anglais
                { "search", "I can help you find the procedure. What is the title or subject you're looking for?" },
                { "help", "I can help you search for procedures. Here are some frequently asked questions:\n" }
            };
        }

        public async Task<ChatMessage> ProcessMessageAsync(string userId, string message)
        {
            var language = DetectLanguage(message);
            var intent = DetectIntent(message, language);
            
            string response;
            string relatedProcedureId = null;

            if (intent == "recherche" || intent == "search")
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

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return chatMessage;
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

        public async Task<List<FrequentlyAskedQuestion>> GetFrequentlyAskedQuestionsAsync(string language)
        {
            return await _context.FrequentlyAskedQuestions
                .Where(f => f.Language == language && f.IsActive)
                .OrderByDescending(f => f.UsageCount)
                .Take(5)
                .ToListAsync();
        }

        public async Task AddFeedbackAsync(int chatMessageId, string userId, bool isHelpful, string comment)
        {
            var feedback = new ChatFeedback
            {
                ChatMessageId = chatMessageId,
                UserId = userId,
                IsHelpful = isHelpful,
                Comment = comment,
                Timestamp = DateTime.UtcNow,
                Language = (await _context.ChatMessages.FindAsync(chatMessageId)).Language
            };

            _context.ChatFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            // Mise à jour du compteur d'utilisation pour les FAQ
            var chatMessage = await _context.ChatMessages.FindAsync(chatMessageId);
            if (chatMessage != null && !string.IsNullOrEmpty(chatMessage.RelatedProcedureId))
            {
                var faq = await _context.FrequentlyAskedQuestions
                    .FirstOrDefaultAsync(f => f.Question == chatMessage.Message);
                if (faq != null)
                {
                    faq.UsageCount++;
                    faq.LastUpdated = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
        }

        private string DetectLanguage(string message)
        {
            if (Regex.IsMatch(message, @"[éèêëàâäôöûüçîï]"))
                return "fr";
            return "en";
        }

        private string DetectIntent(string message, string language)
        {
            message = message.ToLower();
            foreach (var pattern in _searchPatterns)
            {
                if (Regex.IsMatch(message, pattern.Value, RegexOptions.IgnoreCase))
                {
                    return pattern.Key;
                }
            }
            return language == "fr" ? "aide" : "help";
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string userId)
        {
            return await _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
} 