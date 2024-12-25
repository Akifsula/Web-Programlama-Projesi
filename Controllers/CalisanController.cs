using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;
using KuaforYonetim.Data;
using Microsoft.EntityFrameworkCore;
using KuaforYonetim.ViewModels;

namespace KuaforYonetim.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalisanController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Detay(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.CalisanHizmetler)
                .ThenInclude(ch => ch.Hizmet)
                .Include(c => c.CalisanUygunluklar)
                .FirstOrDefault(c => c.CalisanId == id);

            if (calisan == null)
            {
                return NotFound();
            }

            var doluRandevular = _context.Randevular
                .Where(r => r.CalisanId == id && r.Durum == RandevuDurumu.Onaylandi)
                .ToList();

            var viewModel = new CalisanDetayViewModel
            {
                Calisan = calisan,
                DoluRandevular = doluRandevular
            };

            return View(viewModel);
        }


        public IActionResult Index()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanHizmetler) // CalisanHizmetler ilişkisinin yüklenmesi
                .ThenInclude(ch => ch.Hizmet)    // Hizmet detaylarının da yüklenmesi
                .ToList();

            return View(calisanlar);
        }



        public IActionResult Ekle() => View();

        [HttpPost]
        public IActionResult Ekle(Calisan calisan)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(calisan); // Model hatalıysa aynı görünümü tekrar döndür.
            }

            _context.Calisanlar.Add(calisan);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}