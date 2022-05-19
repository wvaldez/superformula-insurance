using InsurancePolicies.API.Models;
using InsurancePolicies.Core.Entities;
using System;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Services
{
    public class StateRegulator : IStateRegulator
    {
        public Task<StateRegulationResult> ValidRegulation(InsurancePolicy policy)
        {
            // Testing purpose, validate synchronous functionality
            Task.Delay(2000).Wait();

            var validationResult = new StateRegulationResult { Reason = "Failed regulation due to invalid data", Status = false };
            if (new Random().Next(1, 2) == 1)
            {
                validationResult = new StateRegulationResult { Reason = "Success regulation due to valid data", Status = true };
            }

            return Task.FromResult(validationResult);
        }
    }
}
