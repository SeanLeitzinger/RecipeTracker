using FluentValidation;
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

    public class ApplicationUserValidator : AbstractValidator<ApplicationUser>
    {
        public ApplicationUserValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(100);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
        }
    }
}
