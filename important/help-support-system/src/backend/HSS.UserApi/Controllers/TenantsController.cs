using HSS.SharedServices.Tenants;
using HSS.SharedServices.Tenants.Contracts;
using HSS.SharedServices.Tenants.Services;
using Microsoft.AspNetCore.Mvc;

namespace HSS.UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost("query")]
        public async Task<IList<TenantModel>> Index(GetAllTenantsRequest request)
        {
            return await _tenantService.GetAllTenantsAsync(request);
        }
    }
}