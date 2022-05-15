using InsurancePolicies.Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly IInsuranceDbContext _database;

        public Repository(IInsuranceDbContext database)
        {
            _database = database;
        }
        public async Task AddAsync(T entity)
        {
            await _database.Set<T>().AddAsync(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _database.Set<T>();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _database.Set<T>().SingleOrDefaultAsync(p => p.Id == id);
        }

        public Task Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _database.SaveChangesAsync();
        }
    }
}
