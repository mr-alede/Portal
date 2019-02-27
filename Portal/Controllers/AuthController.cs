using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Portal.ApiModel.Auth;
using Portal.Helpers;
using Portal.Model.Identity;
using Portal.Security;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<PortalUser> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;

        public AuthController(UserManager<PortalUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginApiModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await identity.GenerateJwt(jwtFactory, credentials.UserName, jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.Claims.Single(c => c.Type == "id");
            var user = await userManager.FindByIdAsync(userId.Value);

            return new OkObjectResult(new
            {
                user.UserName,
                user.Email
            });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}