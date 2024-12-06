using Microsoft.AspNetCore.Identity;

namespace KuaforYonetim.Models
{
    public class Kullanici : IdentityUser
    {
        public string AdSoyad { get; set; }
    }
}