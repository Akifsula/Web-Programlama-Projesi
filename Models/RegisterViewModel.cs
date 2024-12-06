using System.ComponentModel.DataAnnotations;

namespace KuaforYonetim.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}