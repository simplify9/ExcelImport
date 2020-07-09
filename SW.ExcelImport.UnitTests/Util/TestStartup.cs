using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SW.PrimitiveTypes;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using SW.ExcelImport;
using SW.ExcelImport.IServiceCollectionExtensions;

namespace SW.ExcelImport.UnitTests
{
    public class TestStartup
    {
        readonly IConfiguration configuration;
        
        readonly ILoggerFactory loggerFactory;
        public TestStartup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.loggerFactory = loggerFactory;

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExcelImport();
            services.AddScoped<ICloudFilesService, LocalReader>();
            services.AddDbContext<DbContext, ExcelDbContext>(c =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                c.UseSqlite(connection);
            },
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);
        }

        
        public void Configure(IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbc = scope.ServiceProvider.GetService<DbContext>();

                dbc.Database.EnsureCreated();

                

                dbc.SaveChanges();
            }
        }
    }
}
