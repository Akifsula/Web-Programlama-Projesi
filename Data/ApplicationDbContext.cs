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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Hizmet tablosundaki 'Ucret' sütunu için tür tanımı
            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Hizmet>().HasData(
    new Hizmet
    {
        HizmetId = 1,
        Ad = "Saç Kesimi",
        Ucret = 250,
        TahminiSure = TimeSpan.FromMinutes(30)
    },
    new Hizmet
    {
        HizmetId = 2,
        Ad = "Sakal Traşı",
        Ucret = 100,
        TahminiSure = TimeSpan.FromMinutes(10)
    }
);




            // Çalışanlar için başlangıç verileri (Seed Data)
            modelBuilder.Entity<Calisan>().HasData(
                new Calisan
                {
                    CalisanId = 1,
                    AdSoyad = "Ahmet Yılmaz",
                    UzmanlikAlanlari = "Saç Kesimi, Sakal Traşı"
                },
                new Calisan
                {
                    CalisanId = 2,
                    AdSoyad = "Mehmet Kaya",
                    UzmanlikAlanlari = "Boyama, Saç Şekillendirme"
                }
            );
        }
    }
}
