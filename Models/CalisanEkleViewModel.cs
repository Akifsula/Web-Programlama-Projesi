using KuaforYonetim.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.ViewModels
{
    public class CalisanEkleViewModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Display(Name = "Uzmanlık Alanları")]
        public List<int> SelectedHizmetler { get; set; } = new List<int>();

        public List<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();
    }
}