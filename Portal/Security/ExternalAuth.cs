using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Security
{
    internal static class ExternalAuth
    {
        public static void ConfigureExternalAuth(this AuthenticationBuilder authBuilder, AppConfiguration.Authentication cfg)
        {
            if (cfg.Twitter != null && cfg.Twitter.Enabled)
            {
                authBuilder
                    .AddTwitter("Twitter", options =>
                    {
                        options.ConsumerKey = cfg.Twitter.AppKey;
                        options.ConsumerSecret = cfg.Twitter.AppSecret;
                    });
            }

            if (cfg.Facebook != null && cfg.Facebook.Enabled)
            {
                authBuilder
                    .AddFacebook("Facebook", options =>
                    {
                        options.AppId = cfg.Facebook.AppKey;
                        options.AppSecret = cfg.Facebook.AppSecret;
                    });
            }

            if (cfg.Google != null && cfg.Google.Enabled)
            {
                authBuilder
                    .AddGoogle("Google", options =>
                    {
                        options.ClientId = cfg.Google.AppKey;
                        options.ClientSecret = cfg.Google.AppSecret;
                    });
            }

            //if (cfg.LinkedIn != null && cfg.LinkedIn.Enabled)
            //{
            //    authBuilder
            //        .AddLinkedIn("LinkedIn", options =>
            //        {
            //            options.ClientId = cfg.LinkedIn.AppKey;
            //            options.ClientSecret = cfg.LinkedIn.AppSecret;
            //        });
            //}

            if (cfg.AzureAD != null && cfg.AzureAD.Enabled)
            {
                authBuilder
                    .AddOpenIdConnect("aad", "Azure AD", options =>
                    {
                        options.Authority = $"https://login.microsoftonline.com/{cfg.AzureAD.TenantId}";
                        options.ClientId = cfg.AzureAD.AppKey;
                        options.ClientSecret = cfg.AzureAD.AppSecret;
                    });
            }
        }
    }
}
