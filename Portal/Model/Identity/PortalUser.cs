using Microsoft.AspNetCore.Identity;

namespace Portal.Model.Identity
{
    public class PortalUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
