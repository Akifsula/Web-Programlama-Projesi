using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public class Randevu
    {
        [Key]
        public int RandevuId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public int CalisanId { get; set; }

        public Calisan Calisan { get; set; }

        [Required]
        public int HizmetId { get; set; }

        public Hizmet Hizmet { get; set; }

        public string KullaniciId { get; set; } // Kullanıcıyla ilişkilendirmek için
        public Kullanici Kullanici { get; set; } // Navigasyon özelliği
    }
}
