using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.Models
{
    public class Islem
    {
        [Key]
        public int IslemId { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Ucret { get; set; }

        [Required]
        public TimeSpan Sure { get; set; }


    }
}