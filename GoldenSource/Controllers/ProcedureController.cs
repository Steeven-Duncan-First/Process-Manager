using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldenSource.Models;
using GoldenSource.Services;

namespace GoldenSource.Controllers
{
    [Authorize]
    public class ProcedureController : Controller
    {
        private readonly IProcedureService _procedureService;
        private readonly IChatbotService _chatbotService;

        public ProcedureController(IProcedureService procedureService, IChatbotService chatbotService)
        {
            _procedureService = procedureService;
            _chatbotService = chatbotService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var procedures = string.IsNullOrEmpty(searchTerm)
                ? new List<Procedure>()
                : await _procedureService.SearchProceduresAsync(searchTerm, userId);

            return View(procedures);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Procedure());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Procedure procedure)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                await _procedureService.CreateProcedureAsync(procedure, userId);
                return RedirectToAction(nameof(Index));
            }

            return View(procedure);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var procedure = await _procedureService.GetProcedureByIdAsync(id, userId);

            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Procedure procedure)
        {
            if (id != procedure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    await _procedureService.UpdateProcedureAsync(procedure, userId);
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }

            return View(procedure);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(int id)
        {
            var result = await _procedureService.ArchiveProcedureAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var procedure = await _procedureService.GetProcedureByIdAsync(id, userId);

            if (procedure == null)
            {
                return NotFound();
            }

            return View(procedure);
        }

        [HttpPost]
        public async Task<IActionResult> ChatbotQuery([FromBody] string query)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var procedures = await _chatbotService.ProcessUserQueryAsync(query, userId);
            return Json(procedures);
        }
    }
} 