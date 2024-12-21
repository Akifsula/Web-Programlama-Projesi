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

        public IActionResult Ekle()
        {
            var calisanlar = _context.Calisanlar.ToList();
            var hizmetler = _context.Hizmetler.ToList();

            ViewBag.Calisanlar = calisanlar;
            ViewBag.Hizmetler = hizmetler;

            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Randevu randevu)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.KullaniciId = userId;

            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu.";
            return RedirectToAction("Randevularim");
        }
    }
}