using Discount.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Domain.Models
{
    public class CustomerDTO
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public SegmentTypesEnum SegmentType { get; set; }
    }
}
