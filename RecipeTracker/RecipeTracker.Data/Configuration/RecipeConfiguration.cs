using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public class RecipeConfiguration : EntityConfiguration<Recipe>
    {
        public override void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.Property(p => p.Name).HasDefaultValue("");
            builder.HasMany(m => m.Comments).WithOne(m => m.Recipe).HasForeignKey(k => k.RecipeId);
            builder.HasMany(m => m.Directions).WithOne(m => m.Recipe).HasForeignKey(k => k.RecipeId);
            builder.HasMany(m => m.Ingredients).WithOne(m => m.Recipe).HasForeignKey(k => k.RecipeId);

            base.Configure(builder);
        }
    }
}
