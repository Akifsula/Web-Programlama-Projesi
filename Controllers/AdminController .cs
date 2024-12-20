using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuaforYonetim.Data;
using KuaforYonetim.Models;

namespace KuaforYonetim.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece admin erişimi
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Çalışan ekleme işlemi
        public IActionResult CalisanEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalisanEkle(Calisan model)
        {
            if (ModelState.IsValid)
            {
                _context.Calisanlar.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}