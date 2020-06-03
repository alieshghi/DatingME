using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Data;

namespace TodoApi.helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultcontext = await next();
            var id = int.Parse(resultcontext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultcontext.HttpContext.RequestServices.GetService<IDatingRepository>();
            var user = await repo.GetUser(id);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}