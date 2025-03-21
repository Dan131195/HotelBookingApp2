using HotelBookingApp2.Models;
using HotelBookingApp2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HotelBookingApp2.Models;

namespace HotelBookingApp2.Controllers
{
    public class PrenotazioneController : Controller
    {
        private readonly PrenotazioneService _prenotazioneService;
        private readonly CameraService _cameraService;
        private readonly ClienteService _clienteService;

        public PrenotazioneController(PrenotazioneService prenotazioneService, CameraService cameraService, ClienteService clienteService)
        {
            _prenotazioneService = prenotazioneService;
            _cameraService = cameraService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var lista = await _prenotazioneService.GetAllAsync(userId, isAdmin);
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> EditModal(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var prenotazione = await _prenotazioneService.GetByIdAsync(id, userId, isAdmin);
            if (prenotazione == null) return NotFound();

            //ViewBag.Clienti = new SelectList(await _clienteService.GetAllAsync(userId, isAdmin), "ClienteId", "Nome", prenotazione.ClienteId);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllAsync(), "CameraId", "Numero", prenotazione.CameraId);

            return PartialView("_EditModal", prenotazione);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateModal(Prenotazione model)
        {
            if (!ModelState.IsValid) return BadRequest("Modello non valido");

            await _prenotazioneService.UpdateAsync(model);
            var updated = await _prenotazioneService.GetByIdAsync(model.PrenotazioneId, model.CreatedById, true);
            return PartialView("_RowPartial", updated);
        }

        // CreateModal & DeleteModal (simili)
    }
}