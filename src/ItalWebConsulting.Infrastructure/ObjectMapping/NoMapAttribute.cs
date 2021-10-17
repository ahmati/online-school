using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ItalWebConsulting.Infrastructure.ObjectMapping
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class NoMapAttribute : Attribute
    {
    }

    public static class IgnoreNoMapExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreMap<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                var descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                var attribute = (NoMapAttribute)descriptor.Attributes[typeof(NoMapAttribute)];
                if (attribute != null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }

        public static IMappingExpression IgnoreMap(this IMappingExpression expression, Type sourceType, Type destinationType)
        {
            
            foreach (var property in sourceType.GetProperties())
            {
                var descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];
                var attribute = (NoMapAttribute)descriptor.Attributes[typeof(NoMapAttribute)];
                if (attribute != null)
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
    }
}
