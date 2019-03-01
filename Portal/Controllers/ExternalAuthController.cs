using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Portal.DataAccess;
using Portal.Helpers;
using Portal.Model.Identity;
using Portal.Security;
using Portal.ViewModels.ExternalAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    [Route("externalAuth")]
    public class ExternalAuthController : Controller
    {
        private readonly PortalDbContext appDbContext;
        private readonly UserManager<PortalUser> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly SignInManager<PortalUser> signInManager;
        private readonly IMapper mapper;

        public ExternalAuthController(
            UserManager<PortalUser> userManager, PortalDbContext appDbContext, IMapper mapper,
            IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, SignInManager<PortalUser> signInManager)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [Route("login/{provider}")]
        public IActionResult Login(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(LoginCallback), "ExternalAuth", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [Route("loginCallback")]
        public async Task<IActionResult> LoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
                return BadRequest($"Error from external provider: {remoteError}");

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest();

            var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user != null)
            {
                return await TokenResult(user);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Name);
            }
            // Lookup if there's an username with this e-mail address in the Db
            user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return await TokenResult(user);
            }

            // Create a unique username using the 'nameidentifier' claim
            var idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var username = $"{info.LoginProvider}-{info.Principal.FindFirst(idKey).Value}";

            user = new PortalUser
            {
                UserName = username,
                Email = email
            };

            await userManager.CreateAsync(user);

            // Remove Lockout and E-Mail confirmation
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;

            // Register this external provider to the user
            await userManager.AddLoginAsync(user, info);

            // Persist everything into the Db
            await appDbContext.SaveChangesAsync();
            return await TokenResult(user);
        }

        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            var user = mapper.Map<PortalUser>(model);
            await userManager.CreateAsync(user);
            await userManager.AddLoginAsync(user, info);

            return await TokenResult(user);
        }

        private async Task<IActionResult> TokenResult(PortalUser user)
        {
            var identity = jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
            var jwt = await identity.GenerateJwt(jwtFactory, user.UserName, jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return Content(
                        "<script type=\"text/javascript\">" +
                        "window.opener.externalProviderLogin(" + JsonConvert.SerializeObject(new { token = jwt, userName = user.Email }) + ");" +
                        "window.close();" +
                        "</script>",
                        "text/html"
                );
        }
    }
}