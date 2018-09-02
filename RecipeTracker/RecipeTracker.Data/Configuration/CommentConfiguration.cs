using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public class CommentConfiguration : EntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.Text).HasDefaultValue("");
            builder.HasOne(m => m.Recipe).WithMany(m => m.Comments).HasForeignKey(k => k.RecipeId);

            base.Configure(builder);
        }
    }
}
