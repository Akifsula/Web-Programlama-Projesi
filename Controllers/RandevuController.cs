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
        public IActionResult Ekle(Randevu randevu)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.KullaniciId = userId;

            // Geçmiş tarih kontrolü
            if (randevu.Tarih < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Geçmiş bir tarih seçemezsiniz.";
                return RedirectToAction("Ekle");
            }

            // Çakışma kontrolü
            var randevuVar = _context.Randevular
                .Any(r => r.CalisanId == randevu.CalisanId && r.Tarih == randevu.Tarih);

            if (randevuVar)
            {
                TempData["ErrorMessage"] = "Bu saat için zaten bir randevu var.";
                return RedirectToAction("Ekle");
            }

            // Randevuyu ekle
            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu.";
            return RedirectToAction("Randevularim");
        }
    }
}