using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSource.Mock.Models;

namespace GoldenSource.Mock.Services
{
    public interface IMockProcedureService
    {
        Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId);
        Task<Procedure> GetProcedureByIdAsync(int id, string userId);
        Task<bool> HasAccessAsync(string userId, int procedureId);
    }

    public class MockProcedureService : IMockProcedureService
    {
        public async Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(500);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return new List<Procedure>();

            var searchTerms = query.ToLower().Split(' ');
            return MockData.Procedures
                .Where(p => !p.IsArchived && 
                           p.AccessLevel <= user.MaxAccessLevel &&
                           searchTerms.Any(term => 
                               p.Title.ToLower().Contains(term) ||
                               p.Department.ToLower().Contains(term) ||
                               p.AffectedServices.ToLower().Contains(term)))
                .ToList();
        }

        public async Task<Procedure> GetProcedureByIdAsync(int id, string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            if (await HasAccessAsync(userId, id))
            {
                return MockData.Procedures.FirstOrDefault(p => p.Id == id && !p.IsArchived);
            }
            return null;
        }

        public async Task<bool> HasAccessAsync(string userId, int procedureId)
        {
            // Simuler un délai réseau
            await Task.Delay(200);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            var procedure = MockData.Procedures.FirstOrDefault(p => p.Id == procedureId);

            return user != null && procedure != null && 
                   !procedure.IsArchived && 
                   user.MaxAccessLevel >= procedure.AccessLevel;
        }
    }
} 