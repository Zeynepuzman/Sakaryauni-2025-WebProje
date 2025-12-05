using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProje_B231210095.Models;

namespace WebProje_B231210095.Data
{
    // IdentityDbContext: ASP.NET Identity kullanıcı yönetimi için tabla şemalarını sağlar.
    // Bizim Uye sınıfımız IdentityUser'dan genişlediği için IdentityDbContext<Uye> kullanıyoruz.
    public class ApplicationDbContext : IdentityDbContext<Uye>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanında oluşturulacak tablolar (DbSet'ler)
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity yapılandırmalarını uygula
            base.OnModelCreating(builder);

            // MANY-TO-MANY (Antrenör ↔ Hizmet)

            // Birleşik anahtar
            builder.Entity<AntrenorHizmet>()
                .HasKey(ah => new { ah.AntrenorId, ah.HizmetId });

            // Antrenör → AntrenorHizmet
            builder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)
                .WithMany(a => a.AntrenorHizmetler)
                .HasForeignKey(ah => ah.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade engellendi

            // Hizmet → AntrenorHizmet
            builder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)
                .WithMany(h => h.AntrenorHizmetler)
                .HasForeignKey(ah => ah.HizmetId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade engellendi


            // RANDEVU İLİŞKİLERİ (Cascade hatalarını önlemek için hepsi Restrict)

            // Randevu → Antrenör
            builder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu → Hizmet
            builder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu → Üye
            builder.Entity<Randevu>()
                .HasOne(r => r.Uye)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
