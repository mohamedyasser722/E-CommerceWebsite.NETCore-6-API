using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; }
        // sortAsc
        public Expression<Func<T, object>> OrderBy { get; }
        // sortDesc
        public Expression<Func<T, object>> OrderByDescending { get; }
        // skip, take
        public int Take { get; }
        public int Skip { get; }
        public bool isPaginationEnabled { get; set; } 

    }
}
