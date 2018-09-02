using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public class IngredientConfiguration : EntityConfiguration<Ingredient>
    {
        public override void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.Property(p => p.Description).HasDefaultValue("");
            builder.HasOne(m => m.Recipe).WithMany(m => m.Ingredients).HasForeignKey(k => k.RecipeId);

            base.Configure(builder);
        }
    }
}
