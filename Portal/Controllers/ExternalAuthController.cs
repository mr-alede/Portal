using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(LoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest($"Error from external provider: {remoteError}");
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest();

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                var identity = jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
                var jwt = await identity.GenerateJwt(jwtFactory, user.UserName, jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                return new OkObjectResult(new { token = jwt, userName = user.UserName });
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                //ViewData["ReturnUrl"] = returnUrl;
                //ViewData["LoginProvider"] = info.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                { Email = info.Principal.FindFirstValue(ClaimTypes.Email) });
            }
        }

        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            var user = mapper.Map<PortalUser>(model);
            await userManager.CreateAsync(user);
            await userManager.AddLoginAsync(user, info);

            var identity = jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
            var jwt = await identity.GenerateJwt(jwtFactory, user.UserName, jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(new { token = jwt, userName = user.UserName });
        }
    }
}