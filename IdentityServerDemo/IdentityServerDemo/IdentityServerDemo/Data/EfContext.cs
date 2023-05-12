using IdentityServerDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerDemo.Data
{
    public class EfContext: IdentityDbContext<IdentityUser>
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedUsers(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            string username = "admin";
            string email = "admin@gmail.com";
            IdentityUser user = new IdentityUser()
            {
                UserName = username.ToLower(),
                NormalizedUserName = username.ToUpper(),
                Email = email.ToLower(),
                NormalizedEmail = email.ToUpper(),
                LockoutEnabled = false,
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Pass@123");
            builder.Entity<IdentityUser>().HasData(user);
        }
    }
}

