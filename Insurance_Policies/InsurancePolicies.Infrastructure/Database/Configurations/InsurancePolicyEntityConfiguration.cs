using InsurancePolicies.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Infrastructure.Database.Configurations
{
    public class InsurancePolicyEntityConfiguration : IEntityTypeConfiguration<InsurancePolicy>
    {
        public void Configure(EntityTypeBuilder<InsurancePolicy> insurancePolicyConfiguration)
        {
            insurancePolicyConfiguration.HasKey(x => x.Id);
            insurancePolicyConfiguration.OwnsOne(i => i.Address);
        }
    }
}
