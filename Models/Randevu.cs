using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public enum RandevuDurumu
    {
        Bekliyor,
        Onaylandi,
        Reddedildi
    }

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

        [Required]
        public string KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        [Required]
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Bekliyor;
    }
}