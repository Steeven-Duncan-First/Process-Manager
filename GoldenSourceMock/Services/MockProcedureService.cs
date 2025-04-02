using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public class MockProcedureService : IMockProcedureService
    {
        public async Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(500);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return Enumerable.Empty<Procedure>();

            return MockData.Procedures
                .Where(p => !p.IsArchived && 
                           p.AccessLevel <= user.MaxAccessLevel &&
                           (string.IsNullOrEmpty(query) || 
                            p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            p.Department.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task<Procedure> GetProcedureByIdAsync(int id, string userId)
        {
            // Simuler un délai réseau
            await Task.Delay(300);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;

            var procedure = MockData.Procedures.FirstOrDefault(p => p.Id == id);
            if (procedure == null || procedure.IsArchived || procedure.AccessLevel > user.MaxAccessLevel)
                return null;

            return procedure;
        }

        public async Task<bool> HasAccessAsync(string userId, int procedureId)
        {
            // Simuler un délai réseau
            await Task.Delay(200);

            var user = MockData.Users.FirstOrDefault(u => u.Id == userId);
            var procedure = MockData.Procedures.FirstOrDefault(p => p.Id == procedureId);

            return user != null && procedure != null && 
                   !procedure.IsArchived && 
                   procedure.AccessLevel <= user.MaxAccessLevel;
        }
    }
} 