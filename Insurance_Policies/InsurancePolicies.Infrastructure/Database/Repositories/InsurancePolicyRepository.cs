using InsurancePolicies.Core.Entities;
using InsurancePolicies.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database.Repositories
{
    public class InsurancePolicyRepository : Repository<InsurancePolicy>, IInsurancePolicyRepository
    {
        private readonly IInsuranceDbContext _database;
        public InsurancePolicyRepository(IInsuranceDbContext database) : base(database)
        {
            _database = database;
        }
    }
}
