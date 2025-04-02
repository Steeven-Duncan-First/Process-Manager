using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public interface IMockLogService
    {
        Task<IEnumerable<AccessLog>> GetUserAccessLogsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<AccessLog> LogAccessAsync(string userId, string action, string details);
        Task<IEnumerable<AccessLog>> GetProcedureAccessLogsAsync(int procedureId);
    }
} 