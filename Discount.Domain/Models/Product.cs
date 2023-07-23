using Discount.Domain.Enums;

namespace Discount.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductCategoryEnum Category { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal DiscountedPrice { get; set; } = 0;
        //mongo üzerinde discount type tut in mem cache yap tekrar almaya gerek yok
        public SegmentTypesEnum? DiscountType { get; set; }

    }
}