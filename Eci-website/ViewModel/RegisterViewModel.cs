using System.ComponentModel.DataAnnotations;

namespace Eci_website.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "E-mail adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Parola gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola onayı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
