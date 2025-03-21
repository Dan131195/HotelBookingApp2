using Microsoft.EntityFrameworkCore;
using HotelBookingApp2.Data;
using HotelBookingApp2.Models;

namespace HotelBookingApp2.Services
{
    public class ClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Metodo compatibile con PrenotazioneController
        public async Task<IEnumerable<Cliente>> GetAllAsync(string userId, bool isAdmin)
        {
            var query = _context.Clienti
                .Include(c => c.CreatedBy)
                .Include(c => c.Prenotazioni)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(c => c.CreatedById == userId);

            return await query.ToListAsync();
        }

        // ✅ Metodo usato nel ClienteController
        public async Task<IEnumerable<Cliente>> GetAllClientsAsync()
        {
            return await _context.Clienti
                .Include(c => c.CreatedBy)
                .Include(c => c.Prenotazioni)
                .ToListAsync();
        }

        // ✅ Per Details, Edit, Delete
        public async Task<Cliente?> GetClienteByIdAsync(Guid id)
        {
            return await _context.Clienti
                .Include(c => c.CreatedBy)
                .Include(c => c.Prenotazioni)
                .FirstOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task CreateClienteAsync(Cliente cliente)
        {
            await _context.Clienti.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(Cliente cliente)
        {
            _context.Clienti.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(Guid id)
        {
            var cliente = await _context.Clienti.FindAsync(id);
            if (cliente != null)
            {
                _context.Clienti.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
