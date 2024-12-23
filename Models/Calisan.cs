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

        
       
        public List<CalisanUygunluk> Uygunluklar { get; set; } = new List<CalisanUygunluk>();

        public List<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
        public ICollection<CalisanHizmet> CalisanHizmetler { get; set; } = new List<CalisanHizmet>();
    }
}

