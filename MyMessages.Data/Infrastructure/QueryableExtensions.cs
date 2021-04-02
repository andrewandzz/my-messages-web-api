using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyMessages.Data.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> IncludeMultiple<TEntity, TProperty>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>>[] navigationProperties) where TEntity : class
        {
            return navigationProperties.Aggregate(source, (query, property) => query.Include(property));
        }
    }
}
