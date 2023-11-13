using Microsoft.EntityFrameworkCore;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
//using MaherAPI.Data;
using WebAPI.Modeles;

namespace WebAPI.Auth
{
    public class ApplicationsDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options) : base(options)
        
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        

        public DbSet<ResetPassword> ResetPasswords { get; set; }
       
    }
}
