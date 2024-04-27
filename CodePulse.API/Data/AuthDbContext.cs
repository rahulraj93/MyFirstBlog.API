using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "502c464f-3ec8-41db-8b3c-9cabd68643d8";
            var writerRoleId = "ea20cf6f-a110-46c1-8641-1f6d6d3895b0";
            //create reader and writer
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Sedd the role
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin role
            var adminUserId = "3763853a-620a-4e63-93e3-fec358f10ff4";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "admin@123");
            
            builder.Entity<IdentityUser>().HasData(admin);

            //Give role to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                }, 
                new IdentityUserRole<string>()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }

            };
        }
    }
}
