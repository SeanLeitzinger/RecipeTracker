using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeTracker.Api.Requests.User;
using System.Threading.Tasks;

namespace RecipeTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost("AddUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
        {
            return await request.HandleAsync();
        }
    }
}
