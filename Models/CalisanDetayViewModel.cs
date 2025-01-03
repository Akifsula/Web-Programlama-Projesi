﻿using KuaforYonetim.Models;

namespace KuaforYonetim.ViewModels
{
    public class CalisanDetayViewModel
    {
        public Calisan Calisan { get; set; }
        public List<Randevu> DoluRandevular { get; set; } = new List<Randevu>();  // yesil kırmızı görünüm ve saat rezerve islemleri icin ayarladım.
    }
}