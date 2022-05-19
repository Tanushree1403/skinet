using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var host= CreateHostBuilder(args).Build();

           //Below Code will create the database when th application starts.
           using(var scope= host.Services.CreateScope()){ // since we are outside the actual code we need to carete a local scope
                var services= scope.ServiceProvider;
                var loggerFactory=services.GetRequiredService<ILoggerFactory>(); // to log any errors in db creating we will carete a loggerFatory obj
                try{
                    var context= services.GetRequiredService<StoreContext>(); //gets the store context scope
                    await context.Database.MigrateAsync(); // will create the db
                    await StoreContextSeed.SeedAsync(context,loggerFactory);// will seed data into database 
                }

                catch(Exception ex){
                    var logger= loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");
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
