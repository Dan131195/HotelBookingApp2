using HotelBookingApp2.Data;
using HotelBookingApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp2.Services
{
    public class CameraService
    {
        private readonly ApplicationDbContext _context;

        public CameraService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Camera>> GetAllAsync()
        {
            return await _context.Camere
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Camera?> GetByIdAsync(Guid id)
        {
            return await _context.Camere
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CameraId == id);
        }

        public async Task CreateAsync(Camera camera)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            _context.Camere.Add(camera);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Camera camera)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            var existingCamera = await _context.Camere.FindAsync(camera.CameraId);
            if (existingCamera == null)
                throw new InvalidOperationException("Camera non trovata.");

            existingCamera.Numero = camera.Numero;
            existingCamera.Tipo = camera.Tipo;
            existingCamera.Prezzo = camera.Prezzo;
            existingCamera.Disponibilita = camera.Disponibilita;

            _context.Camere.Update(existingCamera);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var camera = await _context.Camere.FindAsync(id);
            if (camera != null)
            {
                _context.Camere.Remove(camera);
                await _context.SaveChangesAsync();
            }
        }
    }
}
