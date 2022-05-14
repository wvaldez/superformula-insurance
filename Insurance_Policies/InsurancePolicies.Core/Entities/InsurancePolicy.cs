using InsurancePolicies.Core.Common;
using InsurancePolicies.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Core.Entities
{
    public class InsurancePolicy : Entity, IAggregateRoot
    {
        public DateTime EffectiveDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public Address Address { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Premium { get; set; }
        public Guid VehicleDetailId { get; set; }
        public Vehicle VehicleDetail { get; set; }
    }
}
