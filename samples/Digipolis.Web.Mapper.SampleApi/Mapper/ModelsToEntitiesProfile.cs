using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Mapper
{
    public class ModelsToEntitiesProfile : Profile
    {
        public ModelsToEntitiesProfile() : base("ModelsToEntities")
        {
            CreateMap<Models.ValueEdit, Entities.Value>()
                .ForMember(x => x.Code, x => x.MapFrom(y => y.EditCode))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.EditDescription));
        }
    }
}
