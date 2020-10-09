using System.Collections;
using System.Collections.Generic;

namespace DotnetAcademy.DAL.Models
{
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal VatPercentage { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DetailLine> DetailLines { get; set; }
    }
}
