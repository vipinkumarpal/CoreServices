using CoreServices.Handler;
using CoreServices.Handler.MappingPorfile;
using CoreServices.Infrastructure.Repositories;
using CoreServices.Infrastructure.Repositories.context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Configuration;

namespace CoreServices
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IWebHostEnvironment env)
        {           

            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables()
               .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
           
            //Swagger
            services.AddSwaggerGen(option =>  
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreService API", Version = "v1" });
                option.CustomSchemaIds(x => x.FullName);
            });

            //Database connection
            services.AddDbContext<CoreServicesDataContext>(item => item.UseSqlServer(Configuration.GetConnectionString("CoreservicesDbConnectionString")));

            //Application services of DI
            services.AddScoped<ICoreServiceHandler, CoreServiceHandler>();
            services.AddTransient<ICoreServicesRepository, CoreServicesRepository>();
            
            //AutoMapper
            services.AddAutoMapper(typeof(CoreServiceProfile));
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
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreServices API");
                c.RoutePrefix ="";
            });

            //use CORS
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            //use Routing
            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
