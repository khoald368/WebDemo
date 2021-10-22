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
            var customers = await customerService.GetListAsync();
            return new PartialViewResult
            {
                ViewName = "_CustomerList",
                ViewData = new ViewDataDictionary<List<ViewModels.Customer>>(ViewData, customers)
            };
        }

        public async Task<JsonResult> OnGetCustomerUpdateAsync(int id = 0)
        {
            if (id == 0)
                return await LoadCustomerAsync(new ViewModels.Customer(), true);
            else
            {
                var customer = await customerService.GetAsync(id);
                return await LoadCustomerAsync(customer, true);
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

                return await LoadCustomersAsync();
            }
            else
                return await LoadCustomerAsync(customer, false);
        }

        public async Task<JsonResult> OnPostDeleteAsync(int id)
        {
            await customerService.DeleteAsync(id);

            return await LoadCustomersAsync();
        }

        private async Task<JsonResult> LoadCustomersAsync()
        {
            var customers = await customerService.GetListAsync();
            var html = await renderService.ToStringAsync("_CustomerList", customers);
            return new JsonResult(new { isValid = true, html = html });
        }

        private async Task<JsonResult> LoadCustomerAsync(ViewModels.Customer customer, bool isValid)
        {
            var html = await renderService.ToStringAsync("_CustomerUpdate", customer);
            return new JsonResult(new { isValid = isValid, html = html });
        }
    }
}
