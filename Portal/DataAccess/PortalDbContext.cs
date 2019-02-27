using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal.Model.Identity;

namespace Portal.DataAccess
{
    public class PortalDbContext : IdentityDbContext<PortalUser>
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
