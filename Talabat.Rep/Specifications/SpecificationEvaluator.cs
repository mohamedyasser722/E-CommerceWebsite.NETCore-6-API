using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec) 
        {
            var query = inputQuery; // _context.set<T>()

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria); // _context.set<T>().Where(spec.Criteria)
            }


            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            if(spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            if (spec.isPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take); // _context.set<T>().Skip(spec.Skip).Take(spec.Take)
                spec.isPaginationEnabled = true;
            }


            if(spec?.Includes?.Count() > 0)
            {
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            }   // _context.set<T>().Include(spec.Includes[0]).Include(spec.Includes[1])...

            

            return query;
        }
    }
}
