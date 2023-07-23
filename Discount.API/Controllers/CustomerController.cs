using Discount.Application.Services;
using Discount.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Route("[action]", Name = "AddCustomer")]
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> AddCustomer(CustomerDTO customer)
        {
            await _customerService.AddCustomer(customer);
            return Ok();
        }
    }
}
