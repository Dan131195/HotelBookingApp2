using HotelBookingApp2.Models;
using HotelBookingApp2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // ========== LOGIN ==========
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Password o utente errato.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password o utente errato.");
            }

            return View();
        }

        // ========== LOGOUT ==========
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // ========== REGISTRAZIONE ==========

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(string email, string password, string firstName, string lastName, string ruolo = "User")
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Un utente con questo indirizzo email è già registrato.");
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(ruolo))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = ruolo,
                        NormalizedName = ruolo.ToUpper()
                    });
                }

                await _userManager.AddToRoleAsync(user, ruolo);

                TempData["Messaggio"] = $"✅ Utente {email} creato con successo come {ruolo}.";

                // 🔁 Reindirizza alla lista utenti
                return RedirectToAction("UserList", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<UserListViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserListViewModel
                {
                    Email = user.Email,
                    Ruoli = string.Join(", ", roles)
                });
            }

            return View(userList);
        }
    }
}
