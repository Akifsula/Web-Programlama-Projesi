using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public class CalisanUygunluk
    {
        [Key]
        public int UygunlukId { get; set; }

        [Required]
        public int CalisanId { get; set; }

        public Calisan Calisan { get; set; }

        [Required]
        public string Gun { get; set; } // Örnek: "Pazartesi"

        [DataType(DataType.Time)]
        public TimeSpan BaslangicSaati { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan BitisSaati { get; set; }
    }
}