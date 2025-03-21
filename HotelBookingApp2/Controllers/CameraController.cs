using HotelBookingApp2.Models;
using HotelBookingApp2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingApp2.ViewModels;


namespace HotelBookingApp2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CameraController : Controller
    {
        private readonly CameraService _cameraService;

        public CameraController(CameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public async Task<IActionResult> Index()
        {
            var camere = await _cameraService.GetAllAsync();
            return View(camere);
        }

        public IActionResult Create()
        {
            return View(new CameraViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CameraViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var camera = new Camera
                {
                    CameraId = Guid.NewGuid(),
                    Numero = vm.Numero,
                    Tipo = vm.Tipo,
                    Prezzo = vm.Prezzo,
                    Disponibilita = vm.Disponibilita
                };
                await _cameraService.CreateAsync(camera);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var camera = await _cameraService.GetByIdAsync(id);
            if (camera == null)
                return NotFound();

            var vm = new CameraViewModel
            {
                CameraId = camera.CameraId,
                Numero = camera.Numero,
                Tipo = camera.Tipo,
                Prezzo = camera.Prezzo,
                Disponibilita = camera.Disponibilita
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CameraViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var camera = new Camera
                {
                    CameraId = vm.CameraId.Value,
                    Numero = vm.Numero,
                    Tipo = vm.Tipo,
                    Prezzo = vm.Prezzo,
                    Disponibilita = vm.Disponibilita
                };

                await _cameraService.UpdateAsync(camera);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var camera = await _cameraService.GetByIdAsync(id);
            if (camera == null)
                return NotFound();

            var vm = new CameraViewModel
            {
                CameraId = camera.CameraId,
                Numero = camera.Numero,
                Tipo = camera.Tipo,
                Prezzo = camera.Prezzo,
                Disponibilita = camera.Disponibilita
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _cameraService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
