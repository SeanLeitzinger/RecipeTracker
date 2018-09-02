using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeTracker.Core.Models;
using RequestInjector.NetCore;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeTracker.Api.Request.User
{
    public class AddUserRequest : Request, IRequest, IRequestHandlerAsync<AddUserRequest, IActionResult>
    {
        public ApplicationUser User { get; set; }
        public string Password { get; set; }

        UserManager<ApplicationUser> userManager;
        IHttpContextAccessor httpContextAccessor;

        public AddUserRequest(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> HandleAsync()
        {
            User.IsApproved = false;
            User.UserName = User.Email;
            var name = httpContextAccessor.HttpContext.User.FindFirst("FullName")?.Value;

            var result = string.IsNullOrEmpty(Password) ? await userManager.CreateAsync(User) : await userManager.CreateAsync(User, Password);

            if (result.Succeeded)
                return new OkObjectResult(User);

            return new BadRequestObjectResult(result.Errors.Select(m => m.Description).ToList());
        }
    }
}
