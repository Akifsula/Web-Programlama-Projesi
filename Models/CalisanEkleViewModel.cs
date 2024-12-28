using KuaforYonetim.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//Admin Tarafindan sisteme tanimlanacak yeni calisanlar icin

namespace KuaforYonetim.ViewModels
{
    public class CalisanEkleViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "En az bir uzmanlık alanı seçmelisiniz.")]
        [Display(Name = "Uzmanlık Alanları")]
        public List<int> SelectedHizmetler { get; set; } = new List<int>();

        public List<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();

        [Required(ErrorMessage = "En az bir uygunluk eklemelisiniz.")]
        public List<CalisanUygunluk> Uygunluklar { get; set; } = new List<CalisanUygunluk>();
    }
}