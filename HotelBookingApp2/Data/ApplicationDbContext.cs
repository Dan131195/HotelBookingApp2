using HotelBookingApp2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Camera> Camere { get; set; }
        public DbSet<Prenotazione> Prenotazioni { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Prenotazione>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Prenotazioni)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prenotazione>()
                .HasOne(p => p.Camera)
                .WithMany(c => c.Prenotazioni)
                .HasForeignKey(p => p.CameraId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );
        }
    }
}
