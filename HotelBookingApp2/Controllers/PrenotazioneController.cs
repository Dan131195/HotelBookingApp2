using HotelBookingApp2.Models;
using HotelBookingApp2.Services;
using HotelBookingApp2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HotelBookingApp2.Controllers
{
    [Authorize]
    public class PrenotazioneController : Controller
    {
        private readonly PrenotazioneService _prenotazioneService;
        private readonly CameraService _cameraService;
        private readonly ClienteService _clienteService;

        public PrenotazioneController(
            PrenotazioneService prenotazioneService,
            CameraService cameraService,
            ClienteService clienteService)
        {
            _prenotazioneService = prenotazioneService;
            _cameraService = cameraService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var prenotazioni = await _prenotazioneService.GetAllAsync(userId, isAdmin);
            return View(prenotazioni);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var prenotazione = await _prenotazioneService.GetByIdAsync(id, userId, isAdmin);
            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var camere = await _cameraService.GetAllAsync();
            var clienti = await _clienteService.GetAllAsync(userId, isAdmin);

            var vm = new PrenotazioneViewModel
            {
                DataInizio = DateTime.Today,
                DataFine = DateTime.Today.AddDays(1),
                Camere = new SelectList(camere, "CameraId", "Tipo"),
                Clienti = new SelectList(clienti, "ClienteId", "Cognome")
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrenotazioneViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!ModelState.IsValid)
            {
                vm.Camere = new SelectList(await _cameraService.GetAllAsync(), "CameraId", "Tipo", vm.CameraId);
                vm.Clienti = new SelectList(await _clienteService.GetAllAsync(userId, isAdmin), "ClienteId", "Cognome", vm.ClienteId);
                return View(vm);
            }

            var prenotazione = new Prenotazione
            {
                ClienteId = vm.ClienteId,
                CameraId = vm.CameraId,
                DataInizio = vm.DataInizio,
                DataFine = vm.DataFine,
                Stato = vm.Stato,
                CreatedById = userId
            };

            await _prenotazioneService.CreateAsync(prenotazione);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var prenotazione = await _prenotazioneService.GetByIdAsync(id, userId, isAdmin);
            if (prenotazione == null) return NotFound();

            var camere = await _cameraService.GetAllAsync();
            var clienti = await _clienteService.GetAllAsync(userId, isAdmin);

            var vm = new PrenotazioneViewModel
            {
                PrenotazioneId = prenotazione.PrenotazioneId,
                ClienteId = prenotazione.ClienteId,
                CameraId = prenotazione.CameraId,
                DataInizio = prenotazione.DataInizio,
                DataFine = prenotazione.DataFine,
                Stato = prenotazione.Stato,
                Camere = new SelectList(camere, "CameraId", "Tipo", prenotazione.CameraId),
                Clienti = new SelectList(clienti, "ClienteId", "Cognome", prenotazione.ClienteId)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PrenotazioneViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!ModelState.IsValid)
            {
                vm.Camere = new SelectList(await _cameraService.GetAllAsync(), "CameraId", "Tipo", vm.CameraId);
                vm.Clienti = new SelectList(await _clienteService.GetAllAsync(userId, isAdmin), "ClienteId", "Cognome", vm.ClienteId);
                return View(vm);
            }

            var prenotazione = await _prenotazioneService.GetByIdAsync(vm.PrenotazioneId, userId, isAdmin);
            if (prenotazione == null) return NotFound();

            prenotazione.ClienteId = vm.ClienteId;
            prenotazione.CameraId = vm.CameraId;
            prenotazione.DataInizio = vm.DataInizio;
            prenotazione.DataFine = vm.DataFine;
            prenotazione.Stato = vm.Stato;

            await _prenotazioneService.UpdateAsync(prenotazione);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var prenotazione = await _prenotazioneService.GetByIdAsync(id, userId, isAdmin);
            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _prenotazioneService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
