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

        // Randevularımı Listele
        public IActionResult Randevularim()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Randevularınızı görmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }
            // Giriş yapan kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcıya ait randevuları getir
            var randevular = _context.Randevular
                .Where(r => r.KullaniciId == userId) // Kullanıcı ID'sine göre filtrele
                .Include(r => r.Calisan)
                .Include(r => r.Hizmet)
                .ToList();

            return View(randevular);
        }

        // Yeni Randevu Ekle Sayfası
        public IActionResult Ekle()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Bu işlem için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            // Çalışan ve hizmet listeleri veritabanından alınır
            var calisanlar = _context.Calisanlar.ToList();
            var hizmetler = _context.Hizmetler.ToList();

            ViewBag.Calisanlar = calisanlar; // Çalışanlar ViewBag'e atanır
            ViewBag.Hizmetler = hizmetler; // Hizmetler ViewBag'e atanır

            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Randevu randevu)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.KullaniciId = userId;

            // Uygunluk kontrolü
            var cakisma = _context.Randevular.Any(r =>
                r.CalisanId == randevu.CalisanId &&
                r.Tarih == randevu.Tarih);
            if (cakisma)
            {
                TempData["Error"] = "Seçtiğiniz tarih ve saatte başka bir randevu bulunuyor.";
                return RedirectToAction("Ekle");
            }

            _context.Randevular.Add(randevu);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Randevunuz başarıyla alındı.";
            return RedirectToAction("Randevularim");
        }
    }
}