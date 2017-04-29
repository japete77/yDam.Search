using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger.Model;
using yDevs.Dam.Services.Assets;
using yDevs.Dam.Services.Database;
using yDevs.Config;
using yDevs.Shared.Exceptions;
using yDevs.Shared.Logging;
using Microsoft.AspNetCore.Http;
using yDevs.Services.Logger;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Options;
using yDam.Services.Models;

namespace yDevs.Dam
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DI initialisation
            services.AddSingleton(typeof(IMongoDbContext), typeof(MongoDbContext));
            services.AddSingleton(typeof(ILoggerService), typeof(LoggerService));
            services.AddScoped(typeof(IAssetService), typeof(AssetService));
            services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddScoped(typeof(LoggingFilter), typeof(LoggingFilter));
            services.AddScoped(typeof(IModelsService), typeof(ModelsService));

            // Application config
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            var serviceProvider = services.BuildServiceProvider();
            LoggingFilter loggingFilter = serviceProvider.GetService<LoggingFilter>();

            // Add framework services.
            services.AddMvc(options => 
            {                
                options.Filters.Add(loggingFilter);
            });
            
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "DAM API",
                    Description = "DAM Services",
                    TermsOfService = "NA",
                    Contact = new Contact() { Name="yDevs", Email="info@ydevs.com", Url="http://www.ydevs.com" }
                });
            });            

            // Enable CORS
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials())   
            );            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ILoggerFactory loggerFactory (param)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AppSettings> settings)
        {
            loggerFactory.AddSerilog();            
     
            app.UseHttpException();

            app.UseStaticFiles();
            
            app.UseCors("CorsPolicy");
            
            if (settings.Value.EnableSwagger)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();

                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseSwaggerUi();
            }

            app.UseMvc();
        }
    }
}
