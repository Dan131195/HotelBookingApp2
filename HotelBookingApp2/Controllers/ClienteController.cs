using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingApp2.Models;
using HotelBookingApp2.Services;
using HotelBookingApp2.ViewModels;
using Microsoft.AspNetCore.Identity;
using HotelBookingApp2.Models;

namespace HotelBookingApp2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClienteController(ClienteService clienteService, UserManager<ApplicationUser> userManager)
        {
            _clienteService = clienteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var clienti = await _clienteService.GetAllClientsAsync();
            return View(clienti);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            return cliente == null ? NotFound() : View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var cliente = new Cliente
                {
                    ClienteId = Guid.NewGuid(),
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    Email = model.Email,
                    Telefono = model.Telefono,
                    CreatedById = user?.Id
                };

                await _clienteService.CreateClienteAsync(cliente);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null) return NotFound();

            var model = new EditClienteViewModel
            {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome,
                Cognome = cliente.Cognome,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditClienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cliente = await _clienteService.GetClienteByIdAsync(model.ClienteId);
            if (cliente == null) return NotFound();

            cliente.Nome = model.Nome;
            cliente.Cognome = model.Cognome;
            cliente.Email = model.Email;
            cliente.Telefono = model.Telefono;

            await _clienteService.UpdateClienteAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            return cliente == null ? NotFound() : View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _clienteService.DeleteClienteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
