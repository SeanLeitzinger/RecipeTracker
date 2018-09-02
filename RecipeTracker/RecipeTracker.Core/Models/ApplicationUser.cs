using Microsoft.AspNetCore.Identity;
using System;

namespace RecipeTracker.Core.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public bool IsApproved { get; set; }
        public string LastName { get; set; }
    }
}
