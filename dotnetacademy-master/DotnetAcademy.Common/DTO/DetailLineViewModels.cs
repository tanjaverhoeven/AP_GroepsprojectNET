using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotnetAcademy.Common.DTO
{

    public class DetailLineViewModel
    {
        public int Id { get; set; }
        public decimal Discount { get; set; }
        public int Amount { get; set; }
        public bool Deleted { get; set; }
        public string DeleteMessage { get; set; }

        [JsonIgnore]
        public InvoiceViewModel Invoice { get; set; }
        public int InvoiceId { get; set; }

        public ProductViewModel Product { get; set; }
        public int ProductId { get; set; }
    }
}
