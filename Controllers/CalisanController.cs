using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Models;
using KuaforYonetim.Data;

namespace KuaforYonetim.Controllers
{
    public class CalisanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalisanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var calisanlar = _context.Calisanlar.ToList();
            return View(calisanlar);
        }

        public IActionResult Ekle() => View();

        [HttpPost]
        public IActionResult Ekle(Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(calisan);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calisan);
        }
    }
}