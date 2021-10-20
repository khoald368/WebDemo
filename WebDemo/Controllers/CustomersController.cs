using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebDemo.Services;

namespace WebDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await customerService.GetListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await customerService.GetAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ViewModels.Customer customer)
        {
            await customerService.AddAsync(customer);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ViewModels.Customer customer)
        {
            await customerService.UpdateAsync(customer);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await customerService.DeleteAsync(id);
            return Ok();
        }
    }
}
