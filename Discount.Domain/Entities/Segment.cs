using Discount.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities
{
    public class Segment
    {
        [Key]
        public string Type { get; set; }
        public decimal DiscountRate { get; set; }

        public virtual List<Customer> Customers { get; set; }

    }
}
