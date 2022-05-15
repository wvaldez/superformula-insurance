using InsurancePolicies.API.Models;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePoliciesController : ControllerBase
    {
        private readonly ILogger<InsurancePoliciesController> _logger;
        private readonly IInsurancePolicyRepository _insurancePolicyRepository;
        public InsurancePoliciesController(ILogger<InsurancePoliciesController> logger, IInsurancePolicyRepository insurancePolicyRepository)
        {
            _logger = logger;
            _insurancePolicyRepository = insurancePolicyRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePolicyRequest policyRequest)
        {
            try
            {
                //TODO: Add validators
                await _insurancePolicyRepository.AddAsync(policyRequest.InsurancePolicy);
                await _insurancePolicyRepository.SaveAsync();
                return Ok(policyRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Insurance Policy {ex.Message}");
                return Problem("Error creating Insurance Policy");
            }

        }

        [HttpGet]
        public async Task<ActionResult<List<InsurancePolicy>>> Get()
        {
            try
            {
                var result = _insurancePolicyRepository.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Insurance Policies {ex.Message}");
                return Problem("Error retrieving Insurance Policies");
            }
        }

        [HttpGet("{policyId}")]
        public async Task<ActionResult<InsurancePolicy>> Get(int policyId)
        {
            try
            {
                var result = await _insurancePolicyRepository.GetAsync(policyId);
                if(result == null)
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
