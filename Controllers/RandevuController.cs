using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Data;
using KuaforYonetim.Models;
using System.Linq;
using System.Security.Claims;

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

            // Giriş yapılmışsa devam eder
            var calisanlar = _context.Calisanlar.ToList();
            var hizmetler = _context.Hizmetler.ToList();

            ViewBag.Calisanlar = calisanlar;
            ViewBag.Hizmetler = hizmetler;

            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Randevu randevu)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Randevu eklemek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcıya ait randevuyu kaydet
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.KullaniciId = userId;

            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            return RedirectToAction("Randevularim");
        }
    }
}