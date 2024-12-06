using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required]
        public string UzmanlikAlanlari { get; set; } // Örnek: "Saç Kesimi, Sakal Traşı"

        public List<CalisanUygunluk> Uygunluklar { get; set; } = new List<CalisanUygunluk>();

        public List<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}

