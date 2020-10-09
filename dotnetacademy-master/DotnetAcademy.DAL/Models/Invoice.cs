using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.DAL.Models {
    public class Invoice {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public string DeleteMessage { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public ICollection<DetailLine> DetailLines { get; set; }
    }
}
