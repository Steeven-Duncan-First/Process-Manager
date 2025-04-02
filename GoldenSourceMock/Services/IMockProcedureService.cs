using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public interface IMockProcedureService
    {
        Task<IEnumerable<Procedure>> SearchProceduresAsync(string query, string userId);
        Task<Procedure> GetProcedureByIdAsync(int id, string userId);
        Task<bool> HasAccessAsync(string userId, int procedureId);
    }
} 