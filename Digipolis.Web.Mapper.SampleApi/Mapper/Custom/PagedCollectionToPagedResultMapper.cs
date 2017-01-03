using AutoMapper;
using Digipolis.Web.Api;
using Digipolis.Web.Mapper.SampleApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digipolis.Web.Mapper.SampleApi.Mapper.Custom
{
    public class PagedCollectionToPagedResultMapper<TEntity, TModel> : ITypeConverter<PagedCollection<TEntity>, PagedResult<TModel>>
    {
        public PagedResult<TModel> Convert(PagedCollection<TEntity> source, PagedResult<TModel> destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            var data = context.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(source.Data);

            var pageOptions = new PageSortOptions()
            {
                Page = source.Page.Number,
                PageSize = source.Page.Size,
                Sort = source.Page.Order
            };

            return new PagedResult<TModel>(pageOptions, (int)source.TotalCount, data);
        }
    }
}
