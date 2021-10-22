using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebDemo.Models;
using WebDemo.Services;

namespace WebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            services.AddRazorPages().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRazorRenderService, RazorRenderService>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddSwaggerGen(o => o.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Demo", Version = "v1", Description = "This is a web demonstration" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Web Demo v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });


        }
    }
}
