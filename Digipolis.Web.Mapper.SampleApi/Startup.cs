using AutoMapper;
using Digipolis.Errors;
using Digipolis.Web.Api;
using Digipolis.Web.Api.ApiExplorer;
using Digipolis.Web.Mapper.Configuration;
using Digipolis.Web.Mapper.ModelBinders;
using Digipolis.Web.Mapper.Options;
using Digipolis.Web.Mapper.SampleApi.Entities;
using Digipolis.Web.Mapper.SampleApi.Logic;
using Digipolis.Web.Mapper.SampleApi.Mapper;
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
using System.Net;
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
            services.AddMvc().AddApiExtensions(build: x =>
            {
                x.PageSize = 2;
                x.DisableGlobalErrorHandling = true;
                x.DisableVersioning = true;
            });

            // Add default response types
            services.AddDefaultResponsesApiDescriptionProvider();
            services.Configure<DefaultResponsesOptions>(options =>
            {
                // IMPORTANT: this order should be lower than the Web.Mapper api description provider order.
                options.Order = 0;

                // All responses produce json
                options.AddDefaultResponseFormat("application/json");

                // 401 - Unauthorized
                options.AddDefaultResponseType((int)HttpStatusCode.Unauthorized, typeof(Error));

                // 403 - Forbidden
                options.AddDefaultResponseType((int)HttpStatusCode.Forbidden, typeof(Error));

                // 404 - Not Found
                options.AddDefaultResponseType((int)HttpStatusCode.NotFound, typeof(Error)).ToGet().WhenHasRouteParameter(paramNameContains: "id");
                options.AddDefaultResponseType((int)HttpStatusCode.NotFound, typeof(Error)).ToPut().WhenHasRouteParameter(paramNameContains: "id");

                // 422 - Validation failed
                options.AddDefaultResponseType(422, typeof(Error)).ToPost();
                options.AddDefaultResponseType(422, typeof(Error)).ToPut();

                // 500 - Internal server Error
                options.AddDefaultResponseType((int)HttpStatusCode.InternalServerError, typeof(Error));
            });

            // Add other description providers
            services.AddApiDescriptionProvider<LowerCaseRelativePathApiDescriptionProvider>(10);
            services.AddApiDescriptionProvider<LowerCaseQueryParametersApiDescriptionProvider>(11);
            services.AddApiDescriptionProvider<ConsumesJsonApiDescriptionProvider>(30);

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

            // Add Mapper
            services.AddWebMapper(autoMapperOptions =>
            {
                autoMapperOptions.AddProfile<EntitiesToModelsProfile>();
            }, actionResultOptions =>
            {
                actionResultOptions.AddMapping<Value, ValueDetail>();
                actionResultOptions.AddGenericMapping(typeof(DataPage<>), typeof(PagedResult<>));

                actionResultOptions.TryAddMappingsFromAutoMapperProfile<EntitiesToModelsProfile>();
            }, 1);
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
