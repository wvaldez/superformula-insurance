using InsurancePolicies.Core.Common;
using InsurancePolicies.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database
{
    public interface IInsuranceDbContext
    {
        DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        DbSet<Vehicle> Vehicles { get; set; }

        DbSet<T> Set<T>() where T : Entity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}