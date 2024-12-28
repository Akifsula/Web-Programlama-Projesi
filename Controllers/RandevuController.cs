using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Data;
using KuaforYonetim.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace KuaforYonetim.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult RandevuTakvimi(int calisanId)
        {
            var uygunluklar = _context.CalisanUygunluklar
                .Where(u => u.CalisanId == calisanId)
                .Select(u => new
                {
                    title = $"Müsait: {u.BaslangicSaati} - {u.BitisSaati}",
                    start = DateTime.Today.AddDays((int)u.Gun).Add(u.BaslangicSaati),
                    end = DateTime.Today.AddDays((int)u.Gun).Add(u.BitisSaati)
                }).ToList();

            var doluZamanlar = _context.Randevular
                .Where(r => r.CalisanId == calisanId && r.Durum == RandevuDurumu.Onaylandi)
                .Select(r => new
                {
                    title = $"Dolu",
                    start = r.Tarih.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = r.Tarih.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss") // Örnek: Randevu sürelerini 1 saat aralıklarla ayarlamaya calıstım
                }).ToList();

            ViewBag.Uygunluklar = uygunluklar;
            ViewBag.DoluZamanlar = doluZamanlar;
            ViewBag.CalisanId = calisanId;

            return View();
        }



        public IActionResult Randevularim()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var randevular = _context.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Hizmet)
                .Where(r => r.KullaniciId == userId)
                .ToList();

            return View(randevular);
        }




        [HttpGet]
        public IActionResult Ekle()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanUygunluklar)
                .Include(c => c.CalisanHizmetler)
                .ThenInclude(ch => ch.Hizmet)
                .ToList();

            var hizmetler = _context.Hizmetler.ToList();
            var doluZamanlar = _context.Randevular
                .Where(r => r.Durum == RandevuDurumu.Onaylandi)
                .Select(r => r.Tarih)
                .ToList();

            ViewBag.Calisanlar = calisanlar;
            ViewBag.Hizmetler = hizmetler;
            ViewBag.DoluZamanlar = doluZamanlar;

            return View();
        }




        [HttpPost]
        public IActionResult Ekle(string Tarih, string Saat, Randevu randevu)
        {
            if (DateTime.TryParse($"{Tarih} {Saat}", out DateTime parsedDateTime))
            {
                randevu.Tarih = parsedDateTime;
            }
            else
            {
                TempData["ErrorMessage"] = "Geçerli bir tarih ve saat giriniz.";
                return RedirectToAction("Ekle");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.KullaniciId = userId;

            // Geçmiş tarih kontrolü
            if (randevu.Tarih < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Geçmiş bir tarih seçemezsiniz.";
                return RedirectToAction("Ekle");
            }

            // Çalışan uygunluğu kontrolü
            var calisanUygunluklari = _context.CalisanUygunluklar
                .Where(cu => cu.CalisanId == randevu.CalisanId)
                .ToList();

            var uygunluk = calisanUygunluklari.Any(cu =>
                cu.Gun == randevu.Tarih.DayOfWeek &&
                randevu.Tarih.TimeOfDay >= cu.BaslangicSaati &&
                randevu.Tarih.TimeOfDay <= cu.BitisSaati);

            if (!uygunluk)
            {
                TempData["ErrorMessage"] = "Bu saat çalışanın uygunluk saatleri arasında değil.";
                return RedirectToAction("Ekle");
            }

            // Sadece onaylanmış randevulara göre çakışma kontrolü
            if (_context.Randevular.Any(r =>
                r.CalisanId == randevu.CalisanId &&
                r.Tarih == randevu.Tarih &&
                r.Durum == RandevuDurumu.Onaylandi)) // Sadece Onaylandı durumundaki randevular kontrol edilir
            {
                TempData["ErrorMessage"] = "Bu saat dolu, lütfen başka bir zaman seçin.";
                return RedirectToAction("Ekle");
            }

            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu.";
            return RedirectToAction("Randevularim");
        }
    }
}