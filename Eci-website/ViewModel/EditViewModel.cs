using System.ComponentModel.DataAnnotations;

namespace Eci_website.ViewModel
{
    public class EditViewModel
    {

        public string? Id { get; set; }
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolalar Eşleşmiyor.")]
        public string? ConfirmPassword { get; set; }

        public IList<string>? SelectedRoles { get; set; }
    }
}