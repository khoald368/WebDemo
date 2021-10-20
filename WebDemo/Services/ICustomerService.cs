using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebDemo.Services
{
    public interface ICustomerService
    {
        Task<List<ViewModels.Customer>> GetListAsync();

        Task<ViewModels.Customer> GetAsync(int id);

        Task AddAsync(ViewModels.Customer customer);

        Task UpdateAsync(ViewModels.Customer customer);

        Task DeleteAsync(int id);
    }
}
