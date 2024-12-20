using Microsoft.AspNetCore.Identity;

namespace KuaforYonetim.Models
{
    // IdentityUser'den türetilen ApplicationUser sınıfı
    public class ApplicationUser : IdentityUser
    {
        // Ekstra özellikler ekleyebilirsiniz
        public string FullName { get; set; } // Örnek: Kullanıcının tam adı
    }
}