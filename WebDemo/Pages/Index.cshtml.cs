using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebDemo.Services;

namespace WebDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService customerService;
        private readonly IRazorRenderService renderService;

        public IndexModel(ICustomerService customerService, IRazorRenderService renderService)
        {
            this.customerService = customerService;
            this.renderService = renderService;
        }

        public async Task<PartialViewResult> OnGetCustomerListPartial()
        {
            Customers = await customerService.GetListAsync();

            return new PartialViewResult
            {
                ViewName = "_CustomerList",
                ViewData = new ViewDataDictionary<List<ViewModels.Customer>>(ViewData, Customers)
            };
        }

        public async Task<JsonResult> OnGetCustomerUpdateAsync(int id = 0)
        {
            if (id == 0)
                return new JsonResult(new { isValid = true, html = await renderService.ToStringAsync("_CustomerUpdate", new ViewModels.Customer()) });
            else
            {
                var customer = await customerService.GetAsync(id);
                return new JsonResult(new { isValid = true, html = await renderService.ToStringAsync("_CustomerUpdate", customer) });
            }
        }

        public async Task<JsonResult> OnPostCustomerUpdateAsync(int id, ViewModels.Customer customer)
        {
            if (ModelState.IsValid)
            {

                if (id == 0)
                    await customerService.AddAsync(customer);
                else
                    await customerService.UpdateAsync(customer);

                Customers = await customerService.GetListAsync();
                var html = await renderService.ToStringAsync("_CustomerList", Customers);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await renderService.ToStringAsync("_CustomerUpdate", customer);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        public List<ViewModels.Customer> Customers { get; set; }
    }
}
