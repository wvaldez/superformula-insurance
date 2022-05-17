using InsurancePolicies.API.Models;
using InsurancePolicies.Core.Entities;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Services
{
    public interface IStateRegulator
    {
        Task<StateRegulationResult> ValidRegulation(InsurancePolicy policy);
    }
}
