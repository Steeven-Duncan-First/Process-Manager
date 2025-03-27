using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenSource.Mock.Models;
using GoldenSource.Mock.Services;

namespace GoldenSource.Mock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcedureController : ControllerBase
    {
        private readonly IMockProcedureService _procedureService;
        private readonly IMockUserService _userService;
        private readonly IMockNotificationService _notificationService;

        public ProcedureController(
            IMockProcedureService procedureService,
            IMockUserService userService,
            IMockNotificationService notificationService)
        {
            _procedureService = procedureService;
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProcedures([FromQuery] string query, [FromQuery] string userId)
        {
            var procedures = await _procedureService.SearchProceduresAsync(query, userId);
            return Ok(procedures);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcedure(int id, [FromQuery] string userId)
        {
            var procedure = await _procedureService.GetProcedureByIdAsync(id, userId);
            if (procedure == null)
                return NotFound();

            // Créer une notification pour le suivi d'accès
            await _notificationService.CreateNotificationAsync(
                userId,
                "Accès à une procédure",
                $"Vous avez accédé à la procédure : {procedure.Title}",
                "Access"
            );

            return Ok(procedure);
        }

        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetProceduresByDepartment(string department)
        {
            var procedures = MockData.Procedures
                .Where(p => p.Department == department && !p.IsArchived)
                .OrderBy(p => p.Title)
                .ToList();

            return Ok(procedures);
        }

        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiringProcedures()
        {
            var expiringProcedures = MockData.Procedures
                .Where(p => !p.IsArchived && 
                           p.ExpirationDate <= DateTime.Now.AddMonths(1))
                .OrderBy(p => p.ExpirationDate)
                .ToList();

            return Ok(expiringProcedures);
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> ArchiveProcedure(int id, [FromQuery] string userId)
        {
            var procedure = MockData.Procedures.FirstOrDefault(p => p.Id == id);
            if (procedure == null)
                return NotFound();

            if (!await _userService.HasRoleAsync(userId, "Administrateur"))
                return Forbid();

            procedure.IsArchived = true;
            procedure.LastModifiedBy = userId;
            procedure.LastModifiedDate = DateTime.Now;

            // Notifier les utilisateurs concernés
            var affectedUsers = await _userService.GetUsersByDepartmentAsync(procedure.Department);
            foreach (var user in affectedUsers)
            {
                await _notificationService.CreateNotificationAsync(
                    user.Id,
                    "Procédure archivée",
                    $"La procédure {procedure.Title} a été archivée.",
                    "Archive"
                );
            }

            return Ok(procedure);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetProcedureStats()
        {
            var stats = new
            {
                TotalProcedures = MockData.Procedures.Count,
                ActiveProcedures = MockData.Procedures.Count(p => !p.IsArchived),
                ExpiringProcedures = MockData.Procedures.Count(p => !p.IsArchived && p.ExpirationDate <= DateTime.Now.AddMonths(1)),
                Departments = MockData.Procedures
                    .Where(p => !p.IsArchived)
                    .GroupBy(p => p.Department)
                    .Select(g => new { Department = g.Key, Count = g.Count() })
            };

            return Ok(stats);
        }

        [HttpGet("access/{id}")]
        public async Task<IActionResult> CheckAccess(int id, [FromQuery] string userId)
        {
            var hasAccess = await _procedureService.HasAccessAsync(userId, id);
            return Ok(new { hasAccess });
        }
    }
} 