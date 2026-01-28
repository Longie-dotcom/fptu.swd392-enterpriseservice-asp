using API.Helper;
using Application.DTO;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EnterprisesController : ControllerBase
    {
        #region Attributes
        private readonly IEnterpriseService enterpriseService;
        #endregion

        #region Properties
        #endregion

        public EnterprisesController(IEnterpriseService enterpriseService)
        {
            this.enterpriseService = enterpriseService;
        }

        #region Methods
        [AuthorizePrivilege("ViewEnterpriseProfile")]
        [HttpGet]
        public async Task<IActionResult> GetEnterprises(
            [FromQuery] QueryEnterpriseDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            var list = await enterpriseService.GetEnterprises(
                dto,
                claims.userId);
            return Ok(list);
        }

        [AuthorizePrivilege("ViewEnterpriseProfile")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEnterpriseDetail(
            Guid id)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            var enterprise = await enterpriseService.GetEnterpriseDetail(
                id,
                claims.userId,
                claims.role);
            return Ok(enterprise);
        }

        [AuthorizePrivilege("CreateEnterpriseCollector")]
        [HttpPost("create-collector")]
        public async Task<IActionResult> CreateMember(
            [FromBody] CreateMemberDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.CreateMember(
                dto,
                claims.userId);
            return Ok("The collector account has been created successfully.");
        }

        [AuthorizePrivilege("AcceptCollectionReport")]
        [HttpPost("accept-collection-report")]
        public async Task<IActionResult> AcceptReport(
            [FromBody] AcceptReportDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.AcceptReport(
                dto,
                claims.userId);
            return Ok("The collection report has been accepted successfully.");
        }
        #endregion
    }
}
