using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItalWebConsulting.Infrastructure.ObjectMapping
{
    public static class IMapperExtensions
    {
        public static async Task<TDestination> MapAsync<TDestination>(this IMapper mapper, object source)
        {
            return await Task.FromResult(mapper.Map<TDestination>(source));
        }
    }
}
