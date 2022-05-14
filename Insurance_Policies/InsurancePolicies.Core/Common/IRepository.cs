using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Core.Common
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> GetAsync(int id);
        Task AddAsync(T entity);
        Task Remove(T entity);
        Task SaveAsync();
    }
}
