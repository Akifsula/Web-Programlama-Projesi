namespace KuaforYonetim.Models

    // Calisanlar ve uzmanlık alanı icin hizmet eşleşmeleri için gerekli degiskenlerim

{
    public class CalisanHizmet
    {
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
    }
}