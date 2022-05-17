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
        public InsurancePoliciesController(ILogger<InsurancePoliciesController> logger, IInsurancePolicyRepository insurancePolicyRepository, IStateRegulator stateRegulator)
        {
            _logger = logger;
            _insurancePolicyRepository = insurancePolicyRepository;
            _stateRegulator = stateRegulator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePolicyRequest policyRequest)
        {
            try
            {
                var validator = new InsurancePolicyValidator().Validate(policyRequest.InsurancePolicy);
                if (!validator.IsValid)
                {
                    return BadRequest(validator.Errors);
                }

                var stateRegulationResult = await _stateRegulator.ValidRegulation(policyRequest.InsurancePolicy);
                if (!stateRegulationResult.Status)
                {
                    _logger.LogError($"Error at state regulation validation");
                    return BadRequest("Error at state regulation validation");
                }

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
