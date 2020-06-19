using ActiveStorage.Sqlite;
using Demo.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ActiveResolver.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo
{
	public class Startup
    {
	    public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddSqliteStorage("sqlite", "Data Source=log.db");
	        services.AddControllersWithViews(o =>
	        {
		        o.Filters.Add(new CancellationTokenFilter());
		        o.Filters.Add(new RequestIdFilter());
	        });
	        services.AddControllers();
	        services.AddGraphViz();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
	            endpoints.MapControllers();
	            endpoints.MapControllerRoute(
		            name: "default",
		            pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
