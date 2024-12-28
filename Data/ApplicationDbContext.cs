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
        public DbSet<CalisanUygunluk> CalisanUygunluklar { get; set; }

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


            // Çalışan Uygunluk ilişkisi
            modelBuilder.Entity<Calisan>()
        .HasMany(c => c.CalisanUygunluklar)
        .WithOne(u => u.Calisan)
        .HasForeignKey(u => u.CalisanId);
            // Uygunluk çakışmalarını engellemek için benzersiz indeks
            modelBuilder.Entity<CalisanUygunluk>()
                .HasIndex(u => new { u.CalisanId, u.Gun, u.BaslangicSaati, u.BitisSaati })
                .IsUnique();



            // Seed data for CalisanUygunluk
            modelBuilder.Entity<CalisanUygunluk>().HasData(
                new CalisanUygunluk
                {
                    UygunlukId = 1,
                    CalisanId = 1,
                    Gun = DayOfWeek.Monday,
                    BaslangicSaati = new TimeSpan(9, 0, 0),
                    BitisSaati = new TimeSpan(17, 0, 0)
                },
                new CalisanUygunluk
                {
                    UygunlukId = 2,
                    CalisanId = 2,
                    Gun = DayOfWeek.Tuesday,
                    BaslangicSaati = new TimeSpan(10, 0, 0),
                    BitisSaati = new TimeSpan(18, 0, 0)
                }
            );

            // Hizmet tablosundaki 'Ucret' sütunu için tür tanımı
            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18,2)");

            // Seed data for Hizmetler
            modelBuilder.Entity<Hizmet>().HasData(
                new Hizmet { HizmetId = 1, Ad = "Saç Kesimi", Ucret = 250, TahminiSure = TimeSpan.FromMinutes(30) },
                new Hizmet { HizmetId = 2, Ad = "Sakal Traşı", Ucret = 100, TahminiSure = TimeSpan.FromMinutes(10) },
                new Hizmet { HizmetId = 3, Ad = "Saç&Sakal", Ucret = 300, TahminiSure = TimeSpan.FromMinutes(40) },
                new Hizmet { HizmetId = 4, Ad = "Saç Boyama", Ucret = 150, TahminiSure = TimeSpan.FromMinutes(60) }
            );




            
            // Seed data for Calisanlar
            modelBuilder.Entity<Calisan>().HasData(
                new Calisan { CalisanId = 1, AdSoyad = "Alperen Akcelik" },
                new Calisan { CalisanId = 2, AdSoyad = "Rauf Sula" }
            );

            // Seed data for CalisanHizmet iliskisi
            modelBuilder.Entity<CalisanHizmet>().HasData(
                new CalisanHizmet { CalisanId = 1, HizmetId = 1 },
                new CalisanHizmet { CalisanId = 1, HizmetId = 2 },
                new CalisanHizmet { CalisanId = 2, HizmetId = 4 }
            );





        }
    }
}
