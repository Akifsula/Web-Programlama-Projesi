using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KuaforYonetim.Models;

namespace KuaforYonetim.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Veri Tabanı Tablolarım
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<CalisanHizmet> CalisanHizmetler { get; set; } // Yeni Ara Tablo

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Çoka çok ilişki yapılandırması
            modelBuilder.Entity<CalisanHizmet>()
                .HasKey(ch => new { ch.CalisanId, ch.HizmetId }); // Birleşik birincil anahtar

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.Calisan)
                .WithMany(c => c.CalisanHizmetler)
                .HasForeignKey(ch => ch.CalisanId);

            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.Hizmet)
                .WithMany(h => h.CalisanHizmetler)
                .HasForeignKey(ch => ch.HizmetId);



            modelBuilder.Entity<Calisan>()
        .HasMany(c => c.CalisanUygunluklar)
        .WithOne(u => u.Calisan)
        .HasForeignKey(u => u.CalisanId);





            // Hizmet tablosundaki 'Ucret' sütunu için tür tanımı
            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18,2)");

            // Seed data for Hizmetler
            modelBuilder.Entity<Hizmet>().HasData(
                new Hizmet { HizmetId = 1, Ad = "Saç Kesimi", Ucret = 250, TahminiSure = TimeSpan.FromMinutes(30) },
                new Hizmet { HizmetId = 2, Ad = "Sakal Traşı", Ucret = 100, TahminiSure = TimeSpan.FromMinutes(10) }
            );




            
            // Seed data for Calisanlar
            modelBuilder.Entity<Calisan>().HasData(
                new Calisan { CalisanId = 1, AdSoyad = "Ahmet Yılmaz" },
                new Calisan { CalisanId = 2, AdSoyad = "Mehmet Kaya" }
            );

            // Seed data for CalisanHizmet (Relation)
            modelBuilder.Entity<CalisanHizmet>().HasData(
                new CalisanHizmet { CalisanId = 1, HizmetId = 1 },
                new CalisanHizmet { CalisanId = 1, HizmetId = 2 },
                new CalisanHizmet { CalisanId = 2, HizmetId = 2 }
            );





        }
    }
}
