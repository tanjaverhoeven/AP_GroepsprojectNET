using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DotnetAcademy.DAL.Models {
    public class Customer {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }

        public string CompanyName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Postal { get; set; }
        public string VatNumber { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Invoice> Invoices { get; set; }

    }
}
