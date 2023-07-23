using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models
{
    public class Bill
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }

    }
}
