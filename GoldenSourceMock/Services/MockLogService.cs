using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSourceMock.Models;

namespace GoldenSourceMock.Services
{
    public class MockLogService : IMockLogService
    {
        private static List<AccessLog> _accessLogs = new List<AccessLog>();

        public async Task<IEnumerable<AccessLog>> GetUserAccessLogsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            await Task.Delay(300);
            var query = _accessLogs.Where(l => l.UserId == userId);
            
            if (startDate.HasValue)
                query = query.Where(l => l.Timestamp >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(l => l.Timestamp <= endDate.Value);
            
            return query.OrderByDescending(l => l.Timestamp);
        }

        public async Task<AccessLog> LogAccessAsync(string userId, string action, string details)
        {
            await Task.Delay(200);
            var log = new AccessLog
            {
                Id = _accessLogs.Count + 1,
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now,
                IpAddress = "127.0.0.1",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"
            };
            _accessLogs.Add(log);
            return log;
        }

        public async Task<IEnumerable<AccessLog>> GetProcedureAccessLogsAsync(int procedureId)
        {
            await Task.Delay(300);
            return _accessLogs
                .Where(l => l.ProcedureId == procedureId)
                .OrderByDescending(l => l.Timestamp);
        }
    }
} 