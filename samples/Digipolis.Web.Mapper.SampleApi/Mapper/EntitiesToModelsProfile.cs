using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.Mapper.SampleApi.Mapper.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Mapper
{
    public class EntitiesToModelsProfile : Profile
    {
        public EntitiesToModelsProfile() : base("EntitiesToModels")
        {
            CreateMap<Entities.DataPage<Entities.Value>, PagedResult<Models.ValueDetail>>()
                .ConvertUsing<DataPageToPagedResultMapper<Entities.Value,Models.ValueDetail>>();

            CreateMap<Entities.Value, Models.ValueDetail>()
                .ForMember(x => x.DetailCode, x => x.MapFrom(y => y.Code))
                .ForMember(x => x.DetailDescription, x => x.MapFrom(y => y.Description));
        }
    }
}
