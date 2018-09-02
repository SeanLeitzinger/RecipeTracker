using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Data.Configuration
{
    public abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(m => m.DateCreated).HasDefaultValueSql(DataConstants.SqlServer.SysDateTime);
            builder.Property(p => p.DateUpdated).HasDefaultValueSql(DataConstants.SqlServer.SysDateTime);
            builder.Property(p => p.UpdatedByName).HasDefaultValue("");
            builder.Property(p => p.CreatedByName).HasDefaultValue("");
            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}
