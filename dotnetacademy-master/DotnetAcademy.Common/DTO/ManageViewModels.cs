using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DotnetAcademy.Common.DTO {
    public class ProfileViewModel {
        public string Username { get; set; }
    }

    public class ChangePasswordViewModel {
        [Required(ErrorMessage = "Oud wachtwoord is verplicht")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nieuw wachtwoord is verplicht")]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,30})", ErrorMessage =
            "Dit is geen geldig wachtwoord, gebruik een wachtwoord tussen 6 en 30 karakters lang dat minstens één hoofdletter, één cijfer en één symbool bevat")]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [Display(Name = "Bevestig wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }
    }
}