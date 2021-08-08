using MCBA_WebAPI.Data;
using MCBA_WebAPI.Models.DataManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace MCBA_WebAPI
{
        public class Startup
        {
            public Startup(IConfiguration configuration) => Configuration = configuration;

            private IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddDbContext<McbaContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(nameof(McbaContext))));

                services.AddScoped<AccountsManager>();
                services.AddScoped<CustomersManager>();
                services.AddScoped<TransactionsManager>();
                services.AddScoped<LoginManager>();
                services.AddScoped<BillPayManager>();
                services.AddControllers();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            // ReSharper disable once UnusedMember.Global
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();

                app.UseRouting();
                app.UseAuthorization();

                app.UseEndpoints(endpoints => endpoints.MapControllers());
            }
        }

    }
