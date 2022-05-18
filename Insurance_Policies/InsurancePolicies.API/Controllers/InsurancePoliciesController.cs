using InsurancePolicies.API.Models;
using InsurancePolicies.API.Services;
using InsurancePolicies.API.Validators;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePoliciesController : ControllerBase
    {
        private readonly ILogger<InsurancePoliciesController> _logger;
        private readonly IInsurancePolicyRepository _insurancePolicyRepository;
        private readonly IStateRegulator _stateRegulator;
        private readonly IMessagePublisher _messagePublisher;
        public InsurancePoliciesController(ILogger<InsurancePoliciesController> logger, IInsurancePolicyRepository insurancePolicyRepository, IStateRegulator stateRegulator, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _insurancePolicyRepository = insurancePolicyRepository;
            _stateRegulator = stateRegulator;
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePolicyRequest policyRequest)
        {
            try
            {
                var validator = new InsurancePolicyValidator().Validate(policyRequest.InsurancePolicy);
                if (!validator.IsValid)
                {
                    return BadRequest(validator.Errors.Select(x => x.ErrorMessage));
                }

                var stateRegulationResult = await _stateRegulator.ValidRegulation(policyRequest.InsurancePolicy);
                if (!stateRegulationResult.Status)
                {
                    _logger.LogError($"Error at state regulation validation");
                    return BadRequest("Error at state regulation validation");
                }

                await _insurancePolicyRepository.AddAsync(policyRequest.InsurancePolicy);
                await _insurancePolicyRepository.SaveAsync();

                _messagePublisher.Publish(policyRequest.InsurancePolicy);
                return Ok(policyRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Insurance Policy {ex.Message}");
                return Problem("Error creating Insurance Policy");
            }

        }

        [HttpGet("licensenumber/{licenseNumber}")]
        public async Task<ActionResult<List<InsurancePolicy>>> GetPoliciesByLicenseNumber(string licenseNumber, [FromQuery] bool sortByYear = false, [FromQuery] bool includeExpiredPolicies = false)
        {
            try
            {
                var result = _insurancePolicyRepository.GetAll().Where(x => x.LicenseNumber.Equals(licenseNumber));
                if (sortByYear)
                {
                    result = result.OrderBy(x => x.VehicleDetail.Year);
                }
                if (includeExpiredPolicies)
                {
                    result = result.Where(x => x.ExpirationDate > DateTime.Now);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Insurance Policies {ex.Message}");
                return Problem("Error retrieving Insurance Policies");
            }
        }

        [HttpGet("id/{policyId}/licenseNumber/{licenseNumber}")]
        public async Task<ActionResult<InsurancePolicy>> GetPolicyById(int policyId, string licenseNumber)
        {
            try
            {
                var result = _insurancePolicyRepository.GetAll().Where(x => x.LicenseNumber.Equals(licenseNumber) && x.Id == policyId).FirstOrDefault();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Insurance Policy {ex.Message}");
                return Problem("Error retrieving Insurance Policy");
            }
        }
    }
}
