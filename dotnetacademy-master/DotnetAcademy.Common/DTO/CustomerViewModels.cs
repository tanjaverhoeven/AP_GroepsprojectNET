using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotnetAcademy.Common.DTO
{
    public class CustomerDetailViewModel
    {
        public CustomerViewModel Customer { get; set; }
        public List<InvoiceViewModel> Invoices { get; set; }
        public bool Deleted { get; set; }
    }
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Id is verplicht.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Emailadres is verplicht.")]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }
        public bool Deleted { get; set; }


        [Required(ErrorMessage = "Plaats is verplicht.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Straat is verplicht.")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string Postal { get; set; }

        public string CompanyName { get; set; }
        [Required(ErrorMessage = "BTW nummer is verplicht.")]
        public string VATNumber { get; set; }

        //[JsonIgnore]
        //public ApplicationUserViewModel ApplicationUser { get; set; }

        public ICollection<InvoiceViewModel> Invoices { get; set; }
    }
    public class CreateCustomerViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Emailadres is verplicht.")]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Plaats is verplicht.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Straat is verplicht.")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string Postal { get; set; }

        [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "BTW nummer is verplicht.")]
        public string VATNumber { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public ICollection<InvoiceViewModel> Invoices { get; set; }
    }
    public class EditCustomerViewModel
    {
        [Required(ErrorMessage = "Id is verplicht.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Emailadres is verplicht.")]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Plaats is verplicht.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Straat is verplicht.")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string Postal { get; set; }

        [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "BTW nummer is verplicht.")]
        public string VATNumber { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public ICollection<InvoiceViewModel> Invoices { get; set; }
    }
    public class DeleteCustomerViewModel
    {
        CustomerViewModel Customer { get; set; }
        public ICollection<InvoiceViewModel> Invoices { get; set; }
    }
}
