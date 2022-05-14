using InsurancePolicies.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.Core.Entities
{
    public class Vehicle:Entity
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
    }
}
