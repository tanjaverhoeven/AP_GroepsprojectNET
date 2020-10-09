using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetAcademy.Common.DTO {

    // Viewmodel for both Customer and ApplicationUser for admin module
    public class UserCustomerViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Display(Name = "Bedrijf")]
        public string CompanyName { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }

        [Display(Name = "Straat")]
        public string Street { get; set; }

        [Display(Name = "Postcode")]
        public string Postal { get; set; }

        [Display(Name = "BTW-nummer")]
        public string VatNumber { get; set; }

        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "E-mail bevestigd?")]
        public bool EmailConfirmed { get; set; }

        public bool Deleted { get; set; }

        public string ApplicationUserId { get; set; }
    }

    public class ApplicationUserViewModel {
        public virtual string Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
    }

    public class LoginViewModel {
        [Required(ErrorMessage = "Gelieve een gebruikersnaam in te geven.")]
        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Gelieve een wachtwoord in te geven.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Onthoud mij")] public bool RememberMe { get; set; }
    }

    public class RegisterViewModel {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }


        [Display(Name = "Bedrijf")]
        public string CompanyName { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }
        [Display(Name = "Straat + nr")]
        public string Street { get; set; }
        [Display(Name = "Postcode")]
        public string Postal { get; set; }

        [Display(Name = "BTW-nummer")]
        public string VatNumber { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam is verplicht.")]
        [StringLength(20, ErrorMessage = "Gebruikersnaam moet tussen {2} en {1} karakters lang zijn", MinimumLength = 4)]
        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-mail adres is verplicht.")]
        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,30})", ErrorMessage =
            "Dit is geen geldig wachtwoord, gebruik een wachtwoord tussen 6 en 30 karakters lang dat minstens één kleine letter, één hoofdletter, één cijfer en één symbool bevat")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel {
        [Required(ErrorMessage = "E-mail adres is verplicht.")]
        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,30})", ErrorMessage =
            "Dit is geen geldig wachtwoord, gebruik een wachtwoord tussen 6 en 30 karakters lang dat minstens één kleine letter, één hoofdletter, één cijfer en één symbool bevat")]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel {
        [Required(ErrorMessage = "E-mail adres is verplicht.")]
        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class EmailConfirmationViewModel {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}