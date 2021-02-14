using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SpendingContext>();
            services.AddScoped<ISpendingRepository, SpendingRepository>();

            services.AddControllers();
            this.DatabaseSetup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // For dev purpose only
        private void DatabaseSetup()
        {
            IConfigurationSection databaseSetupSection = this.Configuration.GetSection("DatabaseSetup");
            using (SpendingContext db = new SpendingContext())
            {
                if (databaseSetupSection["EnsureDeleted"].ToUpperInvariant() == "TRUE")
                {
                    db.Database.EnsureDeleted();
                }
                if (databaseSetupSection["EnsureCreated"].ToUpperInvariant() == "TRUE")
                {
                    db.Database.EnsureCreated();
                }

                db.SaveChanges();
            }
        }
    }
}
