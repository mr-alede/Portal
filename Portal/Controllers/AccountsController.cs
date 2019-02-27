using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.ApiModel.Account;
using Portal.DataAccess;
using Portal.Helpers;
using Portal.Model.Identity;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly PortalDbContext appDbContext;
        private readonly UserManager<PortalUser> userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<PortalUser> userManager, IMapper mapper, PortalDbContext appDbContext)
        {
            this.userManager = userManager;
            _mapper = mapper;
            this.appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<PortalUser>(model);

            var result = await userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await appDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
