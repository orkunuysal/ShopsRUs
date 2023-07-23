using Discount.Domain.Common;

namespace Discount.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string SegmentType { get; set; }

        public Segment Segment { get; set; }

    }
}
