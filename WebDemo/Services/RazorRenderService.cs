using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace WebDemo.Services
{
    public class RazorRenderService : IRazorRenderService
    {
        private readonly IRazorViewEngine razorViewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IHttpContextAccessor httpContext;
        private readonly IActionContextAccessor actionContext;
        private readonly IRazorPageActivator activator;

        public RazorRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IHttpContextAccessor httpContext,
            IRazorPageActivator activator,
            IActionContextAccessor actionContext)
        {
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
            this.httpContext = httpContext;
            this.actionContext = actionContext;
            this.activator = activator;

        }

        public async Task<string> ToStringAsync<T>(string pageName, T model) where T : class
        {
            var actionContext = new ActionContext(httpContext.HttpContext, httpContext.HttpContext.GetRouteData(), this.actionContext.ActionContext.ActionDescriptor);

            using (var sw = new StringWriter())
            {
                var result = razorViewEngine.FindPage(actionContext, pageName);
                if (result.Page == null)
                    throw new ArgumentNullException($"The page {pageName} cannot be found.");

                var view = new RazorView(razorViewEngine, activator, new List<IRazorPage>(), result.Page, HtmlEncoder.Default, new DiagnosticListener("RazorRenderService"));
                var viewData = new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
                var viewTempData = new TempDataDictionary(httpContext.HttpContext, tempDataProvider);
                var viewContext = new ViewContext(actionContext, view, viewData, viewTempData, sw, new HtmlHelperOptions());
                var page = (result.Page);

                page.ViewContext = viewContext;
                activator.Activate(page, viewContext);

                await page.ExecuteAsync();
                return sw.ToString();
            }

        }
    }
}
