using InsurancePolicies.Core.Entities;

namespace InsurancePolicies.API.Models
{
    public class GetInsurancePolicyRequest
    {
        public string PolicyId { get; set; }
        public string LicenseNumber { get; set; }
    }

    public class GetInsurancePoliciesByLicenseRequests
    {
        public string LicenseNumber { get; set; }
    }

    public class CreatePolicyRequest
    {
        public InsurancePolicy InsurancePolicy { get; set; }
    }
}
