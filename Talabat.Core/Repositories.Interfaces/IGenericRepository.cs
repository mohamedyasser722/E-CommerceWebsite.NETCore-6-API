using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Interfaces
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetWithSpecAsync(ISpecifications<T> spec); 
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);

        Task AddAsync(T entity);

        void Delete(T item);
        void Update(T item);

    }
}
