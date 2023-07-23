using Discount.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models
{
    public class SegmentDTO
    {
        public SegmentTypesEnum Type { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
