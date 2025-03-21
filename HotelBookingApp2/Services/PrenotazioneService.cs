
using HotelBookingApp2.Data;
using HotelBookingApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp2.Services
{
    public class PrenotazioneService
    {
        private readonly ApplicationDbContext _context;

        public PrenotazioneService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prenotazione>> GetAllAsync(string userId, bool isAdmin)
        {
            var query = _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(p => p.CreatedById == userId);

            return await query.ToListAsync();
        }

        public async Task<Prenotazione?> GetByIdAsync(Guid id, string userId, bool isAdmin)
        {
            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .FirstOrDefaultAsync(p => p.PrenotazioneId == id);

            if (prenotazione == null) return null;
            if (!isAdmin && prenotazione.CreatedById != userId) return null;

            return prenotazione;
        }

        public async Task CreateAsync(Prenotazione prenotazione)
        {
            _context.Prenotazioni.Add(prenotazione);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prenotazione prenotazione)
        {
            _context.Prenotazioni.Update(prenotazione);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                _context.Prenotazioni.Remove(prenotazione);
                await _context.SaveChangesAsync();
            }
        }
    }
}
