using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public class Hizmet
    {
        [Key]
        public int HizmetId { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; } // Örnek: "Saç Kesimi"

        [Required]
        [Range(0, 1000)]
        public decimal Ucret { get; set; } // Hizmetin ücreti

        [Required]
        public TimeSpan TahminiSure { get; set; } // Tahmini işlem süresi
        public ICollection<CalisanHizmet> CalisanHizmetler { get; set; } = new List<CalisanHizmet>();

    }
}
