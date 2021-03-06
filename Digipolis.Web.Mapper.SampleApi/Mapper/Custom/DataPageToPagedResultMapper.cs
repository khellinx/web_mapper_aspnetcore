﻿using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.Mapper.SampleApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Mapper.Custom
{
    public class DataPageToPagedResultMapper<TEntity, TModel> : ITypeConverter<DataPage<TEntity>, PagedResult<TModel>>
        where TModel : class, new()
    {
        public PagedResult<TModel> Convert(DataPage<TEntity> source, PagedResult<TModel> destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            var items = context.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(source.Data);

            var pageOptions = new PageOptions()
            {
                Page = source.PageNumber,
                PageSize = source.PageLength
            };
            return new PagedResult<TModel>(pageOptions, (int)source.TotalEntityCount, items);
        }
    }
}
