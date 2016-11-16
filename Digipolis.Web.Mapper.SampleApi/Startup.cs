using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.Options;
using Digipolis.Web.Mapper.SampleApi.Entities;
using Digipolis.Web.Mapper.SampleApi.Logic;
using Digipolis.Web.Mapper.SampleApi.Models;
using Digipolis.Web.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi
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
            // Add framework services.
            services.AddMvc(options =>
            {
                // Get required dependencies for the MappedBodyModelBinderProvider
                var serviceProvider = services.BuildServiceProvider();
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                var readerFactory = serviceProvider.GetRequiredService<IHttpRequestStreamReaderFactory>();

                // Get the index of the current BodyModelBinderProvider (which we'll replace).
                var current = options.ModelBinderProviders.OfType<BodyModelBinderProvider>().FirstOrDefault();
                var currentIndex = 0;
                if (current != null)
                {
                    currentIndex = options.ModelBinderProviders.IndexOf(current);
                    options.ModelBinderProviders.RemoveAt(currentIndex);
                }
                options.ModelBinderProviders.Insert(currentIndex, new MappedBodyModelBinderProvider(mapper, options.InputFormatters, readerFactory));
            }).AddApiExtensions(build: x =>
            {
                x.PageSize = 2;
                x.DisableGlobalErrorHandling = true;
                x.DisableVersioning = true;
            });

            // Configure Mapping filter options
            services.Configure<MapResultOptions>(options =>
            {
                options.AddResultMapping<Value, ValueDetail>();
                options.AddGenericResultMapping(typeof(DataPage<>), typeof(PagedResult<>));
            });

            // Add Swagger extensions
            services.AddSwaggerGen<ApiExtensionSwaggerSettings>(x =>
            {
                //Specify Api Versions
                x.MultipleApiVersions(new[] { new Info
                {
                    //Add Inline version
                    Version = "v1",
                    Title = "API V1",
                    Description = "Description for V1 of the API",
                    Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" },
                    TermsOfService = "https://www.digipolis.be/tos",
                    License = new License
                    {
                        Name = "My License",
                        Url = "https://www.digipolis.be/licensing"
                    },
                }});
            });

            // Add logic
            services.AddScoped<IValueLogic, ValueLogic>();

            // Add AutoMapper
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseSwaggerUiRedirect();
        }
    }
}
