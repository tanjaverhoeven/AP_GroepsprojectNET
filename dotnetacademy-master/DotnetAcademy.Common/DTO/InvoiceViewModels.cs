using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.Common.DTO
{
    public class CreateInvoiceViewModel
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public int CustomerId { get; set; }
    }

    public class UpdateInvoiceViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public int CustomerId { get; set; }

    }

    public class DeleteInvoiceViewModel
    {
        public InvoiceViewModel Invoice { get; set; }
        public CustomerViewModel Customer { get; set; }
        public bool HasDetailLines { get; set; }
        public string Message { get; set; }
    }

    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public string DeleteMessage { get; set; }
        public CustomerViewModel Customer { get; set; }
        public int CustomerId { get; set; }
        public ICollection<DetailLineViewModel> DetailLines { get; set; }
    }

    public class InvoiceDetailViewModel
    {
        public InvoiceViewModel Invoice { get; set; }
        public List<DetailLineViewModel> DetailLines { get; set; }
        public CustomerViewModel Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
    }
}
