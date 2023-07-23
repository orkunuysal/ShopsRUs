using Discount.Domain.Entities;
using Discount.Domain.Enums;
using Discount.Domain.Models;
using Discount.Infrastructure.Contracts;
using Discount.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISegmentRepository _segmentRepository;

        public DiscountService(ICustomerRepository customerRepository, ISegmentRepository segmentRepository)
        {
            _customerRepository = customerRepository;
            _segmentRepository = segmentRepository;
        }
        public async Task<Invoice> GenerateInvoiceWithDiscount(Bill bill)
        {
            Invoice invoice = new();
            Customer customerInfo = await _customerRepository.GetByIdAsync(bill.CustomerId);
            Segment segment = await _segmentRepository.GetSegmentByType(customerInfo.SegmentType);
            var loyaltyDiscount = await _segmentRepository.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString());

            List<Product> products = new List<Product>();
            decimal productDiscount = 0;
            foreach (var product in bill.Products)
            {
                if (product.Category != ProductCategoryEnum.Groceries)
                {
                    decimal discountRate = (SegmentTypesEnum)Enum.Parse(typeof(SegmentTypesEnum), segment.Type) == SegmentTypesEnum.Default ?
                                            customerInfo.CreatedDate <= DateTime.UtcNow.AddYears(-2) ? 
                                            loyaltyDiscount.DiscountRate:0 
                                                : segment.DiscountRate;
                    if (discountRate > 0)
                    {
                        product.Discount = product.Price * discountRate / 100;
                        productDiscount += product.Discount;
                        product.DiscountType = (SegmentTypesEnum)Enum.Parse(typeof(SegmentTypesEnum), segment.Type);
                        product.DiscountedPrice = product.Price - product.Discount;
                    }
                }
                products.Add(product);
            }
            invoice.CustomerName = customerInfo.Name;
            invoice.CustomerId = customerInfo.Id;
            invoice.Products = products;
            invoice.Amount = products.Sum(prd => prd.Price);
            invoice.Discount = productDiscount;
            var extraDiscount = Math.Floor((invoice.Amount - invoice.Discount) / 100);
            invoice.ExtraDiscount = extraDiscount > 1 ? extraDiscount * 5: 0;
            invoice.TotalAmount = invoice.Amount - invoice.Discount - invoice.ExtraDiscount;

            return invoice;
        }
    }
}
