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
        [AuthorizePrivilege("ViewEnterprise")]
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

        [AuthorizePrivilege("ViewEnterprise")]
        [HttpGet("{enterpriseId:guid}")]
        public async Task<IActionResult> GetEnterpriseDetail(
            Guid enterpriseId)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            var enterprise = await enterpriseService.GetEnterpriseDetail(
                enterpriseId,
                claims.userId,
                claims.role);
            return Ok(enterprise);
        }

        [AuthorizePrivilege("ViewEnterprise")]
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyEnterpriseProfile(
            Guid enterpriseId)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            var enterprise = await enterpriseService.GetMyEnterpriseProfile(
                claims.userId,
                claims.role);
            return Ok(enterprise);
        }

        [AuthorizePrivilege("ViewWasteType")]
        [HttpGet("waste-type")]
        public async Task<IActionResult> GetWasteTypes()
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            var list = await enterpriseService.GetWasteTypes();
            return Ok(list);
        }

        [AuthorizePrivilege("CreateRewardPolicy")]
        [HttpPost("reward-policy")]
        public async Task<IActionResult> CreateRewardPolicy(
            [FromBody] CreateRewardPolicyDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.CreateRewardPolicy(
                dto,
                claims.userId);
            return Ok("The reward policy has been created successfully.");
        }

        [AuthorizePrivilege("CreateCapacity")]
        [HttpPost("capacity")]
        public async Task<IActionResult> CreateCapacity(
            [FromBody] CreateCapacityDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.CreateCapacity(
                dto,
                claims.userId);
            return Ok("The capacity has been registered successfully.");
        }

        [AuthorizePrivilege("CreateMember")]
        [HttpPost("member")]
        public async Task<IActionResult> CreateMember(
            [FromBody] CreateMemberDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.CreateMember(
                dto,
                claims.userId);
            return Ok("The collector profile has been created successfully.");
        }

        [AuthorizePrivilege("CreateCollectionAssignment")]
        [HttpPost("collection-assignment")]
        public async Task<IActionResult> CreateCollectionAssignment(
            [FromBody] CreateCollectionAssignmentDTO dto)
        {
            var claims = CheckClaimHelper.CheckClaim(User);
            await enterpriseService.CreateCollectionAssignment(
                dto,
                claims.userId);
            return Ok("The collection report has been accepted successfully.");
        }
        #endregion
    }
}
