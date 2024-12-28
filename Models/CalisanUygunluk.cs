using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace KuaforYonetim.Models
{
    public class CalisanUygunluk
    {
        [Key]
        public int UygunlukId { get; set; }

        [Required(ErrorMessage = "Çalışan ID'si zorunludur.")]
        public int CalisanId { get; set; }

        // Nullable olarak tanımlayarak hata almamak icin opsiyonel hale getirdim
        public Calisan? Calisan { get; set; }

        [Required(ErrorMessage = "Gün seçimi zorunludur.")]
        public DayOfWeek Gun { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan BaslangicSaati { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur.")]
        [DataType(DataType.Time)]
        public TimeSpan BitisSaati { get; set; }
    }
}


