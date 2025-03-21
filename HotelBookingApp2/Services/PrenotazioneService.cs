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
            return await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .Include(p => p.CreatedBy)
                .Where(p => isAdmin || p.CreatedById == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Prenotazione?> GetByIdAsync(Guid id, string userId, bool isAdmin)
        {
            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(p => p.PrenotazioneId == id);

            if (prenotazione == null || (!isAdmin && prenotazione.CreatedById != userId))
                return null;

            return prenotazione;
        }

        public async Task CreateAsync(Prenotazione prenotazione)
        {
            if (prenotazione == null)
                throw new ArgumentNullException(nameof(prenotazione));

            var camera = await _context.Camere.FindAsync(prenotazione.CameraId);
            if (camera == null || camera.Numero <= 0)
                throw new InvalidOperationException("La camera selezionata non è disponibile.");

            camera.Numero -= 1;
            _context.Camere.Update(camera);

            prenotazione.PrenotazioneId = Guid.NewGuid();
            _context.Prenotazioni.Add(prenotazione);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Prenotazione prenotazione)
        {
            var existing = await _context.Prenotazioni
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PrenotazioneId == prenotazione.PrenotazioneId);

            if (existing == null)
                throw new InvalidOperationException("Prenotazione non trovata.");

            if (existing.CameraId != prenotazione.CameraId)
            {
                var oldCamera = await _context.Camere.FindAsync(existing.CameraId);
                var newCamera = await _context.Camere.FindAsync(prenotazione.CameraId);

                if (newCamera == null || newCamera.Numero <= 0)
                    throw new InvalidOperationException("La nuova camera selezionata non è disponibile.");

                oldCamera.Numero += 1;
                newCamera.Numero -= 1;

                _context.Camere.Update(oldCamera);
                _context.Camere.Update(newCamera);
            }

            _context.Prenotazioni.Update(prenotazione);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                var camera = await _context.Camere.FindAsync(prenotazione.CameraId);
                if (camera != null)
                {
                    camera.Numero += 1;
                    _context.Camere.Update(camera);
                }

                _context.Prenotazioni.Remove(prenotazione);
                await _context.SaveChangesAsync();
            }
        }
    }
}
