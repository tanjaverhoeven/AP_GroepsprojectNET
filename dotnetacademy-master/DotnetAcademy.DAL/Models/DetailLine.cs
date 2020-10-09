using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.DAL.Models {
    public class DetailLine {
        public int Id { get; set; }
        public decimal Discount { get; set; }
        public int Amount { get; set; }

        public Invoice Invoice { get; set; }
        public int InvoiceId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
