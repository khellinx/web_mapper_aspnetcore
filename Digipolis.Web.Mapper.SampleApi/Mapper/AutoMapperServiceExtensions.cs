using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Mapper
{
    public static class AutoMapperServiceExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntitiesToModelsProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton<IMapper>(mapper);

            return services;
        }
    }
}
