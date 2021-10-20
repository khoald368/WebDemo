using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;
using WebDemo.Models;

namespace WebDemo.Services
{
    public class CustomerService : ICustomerService
    {
        private AppDbContext dbContext;

        public CustomerService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ViewModels.Customer>> GetListAsync()
        {
            return await dbContext.Customers.ProjectToType<ViewModels.Customer>().ToListAsync();
        }

        public async Task<ViewModels.Customer> GetAsync(int id)
        {
            return await dbContext.Customers.ProjectToType<ViewModels.Customer>().Where(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(ViewModels.Customer customer)
        {
            var entity = customer.Adapt<Customer>();
            await dbContext.Customers.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ViewModels.Customer customer)
        {
            var entity = await dbContext.Customers.FindAsync(customer.Id);

            if (entity != null)
            {
                entity.Update(customer);
                dbContext.Customers.Update(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await dbContext.Customers.FindAsync(id);
            if (entity != null)
            {
                dbContext.Customers.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
