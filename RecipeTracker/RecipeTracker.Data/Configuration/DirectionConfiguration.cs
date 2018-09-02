using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public class DirectionConfiguration : EntityConfiguration<Direction>
    {
        public override void Configure(EntityTypeBuilder<Direction> builder)
        {
            builder.Property(p => p.Text).HasDefaultValue("");
            builder.HasOne(m => m.Recipe).WithMany(m => m.Directions).HasForeignKey(k => k.RecipeId);

            base.Configure(builder);
        }
    }
}
