using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KuaforYonetim.Models;

namespace KuaforYonetim.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
    }
}