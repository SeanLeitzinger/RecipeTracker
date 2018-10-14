using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using RecipeTracker.Core.Models;
using RecipeTracker.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeTracker.Api.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> userManager;
        private RecipeTrackerDbContext dbContext;

        public ProfileService(UserManager<ApplicationUser> userManager, RecipeTrackerDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = userManager.GetUserAsync(context.Subject).Result;
            var claims = new List<Claim>
            {
                new Claim("FullName", $"{user.FirstName} {user.LastName}")
            };

            var userClaims = dbContext.UserClaims.Where(m => m.UserId == user.Id).ToList();
            userClaims.ForEach(m => claims.Add(new Claim(m.ClaimType, m.ClaimValue)));
            context.IssuedClaims.AddRange(claims);

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = userManager.GetUserAsync(context.Subject).Result;
            context.IsActive = user != null && user.LockoutEnd == null;

            return Task.FromResult(0);
        }
    }
}
