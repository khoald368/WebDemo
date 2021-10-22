using System.Threading.Tasks;

namespace WebDemo.Services
{
    public interface IRazorRenderService
    {
        Task<string> ToStringAsync<T>(string viewName, T model) where T : class;
    }
}
