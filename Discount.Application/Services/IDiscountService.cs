using Discount.Domain.Models;

namespace Discount.Application.Services
{
    public interface IDiscountService
    {
        Task<Invoice> GenerateInvoiceWithDiscount(Bill bill);
    }
}