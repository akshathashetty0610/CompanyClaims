using CompanyClaims.Models;
using CompanyClaims.Services;
using Microsoft.AspNetCore.Mvc;
using ActiveCompany = CompanyClaims.Models.ActiveCompany;

namespace CompanyClaims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {


        private readonly ILogger<ClaimsController> _logger;
        private readonly ICSVService _csvService;

        public ClaimsController(ILogger<ClaimsController> logger, ICSVService csvService)
        {
            _logger = logger;
            _csvService = csvService;
        }

        

        [HttpPost("GetCompanyById")]
        public async Task<IActionResult> GetCompanyById(Int16 Id, [FromForm] IFormFileCollection companyfile)
        {
            var companies = _csvService.GetCompanyById<ActiveCompany>(Id, companyfile[0].OpenReadStream());

            return Ok(companies);
        }

        [HttpPost("GetClaimsByCompany")]
        public async Task<IActionResult> GetClaimsByCompany(Int16 companyId, [FromForm] IFormFileCollection claimsfile)
        {
            var companies = _csvService.GetClaimsByCompany<IList<ClaimsByCompany>>(companyId, claimsfile[0].OpenReadStream());

            return Ok(companies);
        }

        [HttpPost("GetClaimsWithExpiredDays")]
        public async Task<IActionResult> GetClaimsWithExpiredDays(string claimId, [FromForm] IFormFileCollection claimsfile)
        {
            var companies = _csvService.GetClaimsWithExpiredDays<ClaimsByCompany>(claimId, claimsfile[0].OpenReadStream());

            return Ok(companies);
        }

       
        [HttpPut("UpdateClaim")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClaim([FromForm] ClaimsByCompany claim, [FromForm] IFormFileCollection claimsfile)
        {
            var clai1m = _csvService.UpdateClaim<string>(claim, claimsfile[0]);

            return Ok(clai1m);
        }
    }
}