using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ItalWebConsulting.Infrastructure.ObjectMapping;

namespace ItalWebConsulting.Infrastructure.ObjectMapping
{
    public class MapperConfig: IMapper
    {
        private readonly IMapper iMapper;
        public MapperConfig(IEnumerable<ObjectMapping> objectMappings)
        {
            if (objectMappings == null)
                return;

            var config = new MapperConfiguration(cfg => {
                foreach (var conf in objectMappings)
                {
                    cfg.CreateMap(conf.Source, conf.Destination).IgnoreMap(conf.Source, conf.Destination);
                    if(conf.MapBothDirections)
                        cfg.CreateMap(conf.Destination,conf.Source).IgnoreMap(conf.Source, conf.Destination);
                }
                
            });

            iMapper = config.CreateMapper();
        }

        public IConfigurationProvider ConfigurationProvider => iMapper.ConfigurationProvider;

        public Func<Type, object> ServiceCtor => iMapper.ServiceCtor;

        public TDestination Map<TDestination>(object source)
        {
            return iMapper.Map<TDestination>(source);
        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
        {
            return iMapper.Map<TDestination>(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return iMapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return iMapper.Map<TSource, TDestination>(source, opts);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return iMapper.Map<TSource, TDestination>(source, destination);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            return iMapper.Map<TSource, TDestination>(source, destination, opts);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return iMapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return iMapper.Map(source, sourceType, destinationType, opts);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return iMapper.Map(source, destination, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            return iMapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions<object, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions<object, object>> opts)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return iMapper.ProjectTo(source, parameters, membersToExpand);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object> parameters, params string[] membersToExpand)
        {
            return iMapper.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }

        public IQueryable ProjectTo(IQueryable source, Type destinationType, IDictionary<string, object> parameters = null, params string[] membersToExpand)
        {
            throw new NotImplementedException();
        }
    }
}
