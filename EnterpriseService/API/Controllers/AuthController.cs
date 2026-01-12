using Application.DTO;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Attributes
        private readonly IEnterpriseService enterpriseService;
        #endregion

        #region Properties
        #endregion

        public AuthController(IEnterpriseService enterpriseService)
        {
            this.enterpriseService = enterpriseService;
        }

        #region Methods
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateEnterprise(
            [FromBody] CreateEnterpriseDTO dto)
        {
            await enterpriseService.CreateEnterprise(dto);
            return Ok("The enterprise account has been created successfully.");
        }
        #endregion
    }
}
