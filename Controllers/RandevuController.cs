using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;
using Microsoft.AspNetCore.Authorization;

namespace KuaforYonetim.Controllers
{
    [Authorize] // Bu satır, giriş yapmamış kullanıcıların bu controller'a erişimini engeller.
    public class RandevuController : Controller
    {
        // Örnek randevu ve uygunluk verileri (ileride veritabanından alınacak)
        private static List<Randevu> randevular = new List<Randevu>();
        private static List<CalisanUygunluk> calisanUygunluklar = new List<CalisanUygunluk>
        {
            new CalisanUygunluk { CalisanId = 1, Gun = "Pazartesi", BaslangicSaati = new TimeSpan(9, 0, 0), BitisSaati = new TimeSpan(17, 0, 0) },
            new CalisanUygunluk { CalisanId = 2, Gun = "Salı", BaslangicSaati = new TimeSpan(10, 0, 0), BitisSaati = new TimeSpan(18, 0, 0) }
        };

        public IActionResult Index()
        {
            return View(randevular);
        }
        public IActionResult Ekle(int calisanId)
        {
            // Çalışanları ve hizmetleri veritabanından alabilirsiniz.
            ViewBag.CalisanId = calisanId;
            ViewBag.CalisanAdSoyad = "Ahmet Yılmaz"; // Örnek çalışan adı, veritabanından alınabilir

            return View();
        }
        public IActionResult Ekle()
        {
            // Çalışan ve hizmet listeleri, ileride veritabanından alınacak
            ViewBag.Calisanlar = new List<Calisan>
            {
                new Calisan { CalisanId = 1, AdSoyad = "Ahmet Yılmaz" },
                new Calisan { CalisanId = 2, AdSoyad = "Mehmet Kaya" }
            };

            ViewBag.Hizmetler = new List<Hizmet>
            {
                new Hizmet { HizmetId = 1, Ad = "Saç Kesimi" },
                new Hizmet { HizmetId = 2, Ad = "Sakal Traşı" }
            };

            return View();
        }

        public IActionResult Randevularim()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Önce giriş yapmalısınız. Hesabınız yoksa kaydolmalısınız.";
                return RedirectToAction("Login", "Account"); // Giriş sayfasına yönlendir
            }
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Randevu randevu)
        {
            // Çalışanın uygunluklarını al
            var uygunluklar = calisanUygunluklar.Where(u => u.CalisanId == randevu.CalisanId).ToList();
            var uygun = uygunluklar.Any(u =>
                u.Gun == randevu.Tarih.DayOfWeek.ToString() &&
                randevu.Tarih.TimeOfDay >= u.BaslangicSaati &&
                randevu.Tarih.TimeOfDay <= u.BitisSaati
            );

            if (!uygun)
            {
                ModelState.AddModelError("", "Seçilen saat, çalışanın uygunluk saatleri dışında.");
                return View(randevu);
            }

            // Tarih çakışması kontrolü
            var cakisanRandevu = randevular.FirstOrDefault(r =>
                r.CalisanId == randevu.CalisanId &&
                r.Tarih.Date == randevu.Tarih.Date &&
                r.Tarih.TimeOfDay == randevu.Tarih.TimeOfDay
            );

            if (cakisanRandevu != null)
            {
                ModelState.AddModelError("", "Seçilen saat için başka bir randevu bulunuyor.");
                return View(randevu);
            }

            // Randevu ekle
            randevu.RandevuId = randevular.Count + 1;
            randevular.Add(randevu);
            return RedirectToAction("Index");
        }
    }
}