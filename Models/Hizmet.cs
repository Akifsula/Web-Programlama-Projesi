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
        public decimal Ucret { get; set; } // Hizmetin ücreti (Range degeri yeniden ayarlanabilir)

        [Required]
        public TimeSpan TahminiSure { get; set; } // Tahmini işlem süresi ( en uzun süren islemimi 1 saat olarak ayarladım (sac boyama))
        public ICollection<CalisanHizmet> CalisanHizmetler { get; set; } = new List<CalisanHizmet>();

    }
}
