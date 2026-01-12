using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        #endregion
    }
}
