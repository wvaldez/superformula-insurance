using InsurancePolicies.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancePoliciesController : ControllerBase
    {
        public InsurancePoliciesController()
        {

        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<InsurancePolicy>>> Get()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<InsurancePolicy>> Get(int policyId)
        {
            return Ok();
        }
    }
}
