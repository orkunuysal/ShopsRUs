using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models
{
    public class Invoice
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal ExtraDiscount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
