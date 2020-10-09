using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetAcademy.Common.DTO {
    public class ShoppingCartItemViewModel {
        public ProductViewModel ProductViewModel { get; set; }
        public int Quantity { get; set; }
        public decimal Vat { get; set; }
    }

}
