using InsurancePolicies.Core.Common;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database
{
    public class InsuranceDbContext : DbContext, IInsuranceDbContext
    {
        public InsuranceDbContext(DbContextOptions options) : base(options) { }

        public const string DEFAULT_SCHEMA = "dbo";

        public DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public new DbSet<T> Set<T>() where T : Entity
        {
            return base.Set<T>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "Walterio";
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "Walterio";
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InsurancePolicyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleEntityConfiguration());
        }
    }
}
