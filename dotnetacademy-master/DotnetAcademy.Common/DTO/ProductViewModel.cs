using System.ComponentModel.DataAnnotations;

namespace DotnetAcademy.Common.DTO {
    public class ProductViewModel {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Display(Name = "Level")]
        [Required(ErrorMessage = "Niveau is verplicht")]
        public string Level { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Type is verplicht")]
        public string Type { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Categorie is verplicht")]
        public string Category { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Omschrijving is verplicht")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Prijs is verplicht")]
        [Range(0, int.MaxValue, ErrorMessage = "Gelieve een positief getal in te geven")]
        public decimal Price { get; set; }

        [Display(Name = "BTW %")]
        [Required(ErrorMessage = "BTW is verplicht")]
        [Range(0, 100, ErrorMessage = "BTW % moet tussen {1} en {2} zijn")]
        public decimal VatPercentage { get; set; }

        [Display(Name = "Actief")] public bool IsActive { get; set; }
    }

    public class ProductPerInvoiceViewModel {
        public DetailLineViewModel DetailLine { get; set; }
        public InvoiceViewModel Invoice { get; set; }
    }

    public class SoldProductViewModel {
        public ProductViewModel Product { get; set; }
        public int TotalSold { get; set; }
    }

}