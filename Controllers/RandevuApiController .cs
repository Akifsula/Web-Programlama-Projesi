using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KuaforYonetim.Data;
using KuaforYonetim.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("api/Randevu")]
[ApiController]
public class RandevuApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RandevuApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("uygunluklar/{calisanId}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> GetCalisanUygunluklari(int calisanId)
    {
        try
        {
            var uygunluklar = await _context.CalisanUygunluklar
                .Where(u => u.CalisanId == calisanId)
                .ToListAsync();

            if (!uygunluklar.Any())
            {
                return NotFound("Bu çalışan için uygunluk bilgisi bulunamadı.");
            }

            var doluRandevular = await _context.Randevular
                .Where(r => r.CalisanId == calisanId && r.Durum == RandevuDurumu.Onaylandi)
                .Select(r => r.Tarih)
                .ToListAsync();

            var uygunSaatler = new List<object>();

            foreach (var uygunluk in uygunluklar)
            {
                var baslangic = uygunluk.BaslangicSaati;
                var bitis = uygunluk.BitisSaati;

                var doluSaatler = doluRandevular
                    .Where(r => r.DayOfWeek == uygunluk.Gun)
                    .Select(r => r.TimeOfDay)
                    .ToHashSet();

                for (var saat = baslangic; saat < bitis; saat = saat.Add(TimeSpan.FromHours(1)))
                {
                    if (!doluSaatler.Contains(saat))
                    {
                        uygunSaatler.Add(new
                        {
                            Gun = uygunluk.Gun.ToString(),
                            Saat = saat.ToString(@"hh\:mm")
                        });
                    }
                }
            }

            return Ok(uygunSaatler);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
            return StatusCode(500, "Sunucu hatası.");
        }
    }

    // https://localhost:7100/api/Randevu/uygunluklar/1  adresiyle apiyi test edebilirsin.
}
