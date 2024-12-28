using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    // Randevu durumları icin Enum tanımladım (default degeri randevu alındıgı an "Bekliyor" olacak)
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
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(Randevu), nameof(ValidateTarih))]
        public DateTime Tarih { get; set; }

        [Required]
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        [Required]
        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        [Required]
        [StringLength(450)] 
        public string KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        [Required]
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Bekliyor;

        // Tarih doğrulama metodu
        public static ValidationResult ValidateTarih(DateTime tarih, ValidationContext context)
        {
            if (tarih < DateTime.Now)
            {
                return new ValidationResult("Geçmiş bir tarih seçemezsiniz.");
            }
            return ValidationResult.Success;
        }

    }
}