using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("ApplicationUser");
            builder.Property(p => p.Id).HasDefaultValueSql(DataConstants.SqlServer.NewSequentialId);

            builder.Property(p => p.FirstName).HasDefaultValue("");
            builder.Property(p => p.LastName).HasDefaultValue("");
        }
    }
}
