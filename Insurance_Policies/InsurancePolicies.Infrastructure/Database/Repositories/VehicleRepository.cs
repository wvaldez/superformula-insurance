using InsurancePolicies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database.Repositories
{
    public class VehicleRepository : Repository<Vehicle>
    {
        private readonly IInsuranceDbContext _database;

        public VehicleRepository(IInsuranceDbContext database) : base(database)
        {
            _database = database;
        }
    }
}
