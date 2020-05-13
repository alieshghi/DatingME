using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services= scope.ServiceProvider;
                try
                {
                    var context= services.GetRequiredService<ToDoContext>();
                    context.Database.Migrate();
                    Seed.seedUsers(context);
                }
                catch (Exception ex)
                {
                    var logger= services.GetRequiredService<ILogger<Program>>();
                    logger.LogError (ex,"an error occured during migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
