using Discount.Application.Services;
using Discount.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [Route("[action]", Name = "CreateDiscountInvoice")]
        [HttpPost]
        [ProducesResponseType(typeof(Invoice), (int)HttpStatusCode.OK)]
        public async Task<Invoice> CreateDiscountInvoice(Bill bill)
        {
            return await _discountService.GenerateInvoiceWithDiscount(bill);
        }
    }
}
