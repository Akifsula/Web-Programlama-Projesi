namespace KuaforYonetim.ViewModels
{
    public class UygunlukViewModel
    {
        public int UygunlukId { get; set; }
        public int CalisanId { get; set; }
        public int Gun { get; set; } // DayOfWeek yerine int kullanıyoruz
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }
    }
}