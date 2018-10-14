using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeTracker.Core.Models;
using RecipeTracker.Data.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeTracker.Data
{
    public class RecipeTrackerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private IHttpContextAccessor httpContextAccessor;

        public RecipeTrackerDbContext(DbContextOptions<RecipeTrackerDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Direction> Direction { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new DirectionConfiguration());
            builder.ApplyConfiguration(new IngredientConfiguration());
            builder.ApplyConfiguration(new RecipeConfiguration());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            if (httpContextAccessor != null)
            {
                AddAuditValues();
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (httpContextAccessor != null)
            {
                AddAuditValues();
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddAuditValues()
        {
            var entities = ChangeTracker.Entries().Where(x => (x.Entity is Entity) && (x.State == EntityState.Added || x.State == EntityState.Detached || x.State == EntityState.Modified));
            var sub = httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            var name = httpContextAccessor.HttpContext?.User?.FindFirst("FullName")?.Value;
            Guid.TryParse(sub, out Guid userId);

            if (!string.IsNullOrEmpty(sub))
            {
                foreach (var entity in entities)
                {
                    if (entity.Entity is Entity)
                    {
                        if (entity.State == EntityState.Added || string.IsNullOrEmpty(((Entity)entity.Entity).CreatedByName))
                        {
                            ((Entity)entity.Entity).DateCreated = DateTime.Now;
                            ((Entity)entity.Entity).CreatedBy = userId;
                            ((Entity)entity.Entity).CreatedByName = name;
                        }

                        if (entity.State == EntityState.Modified)
                        {
                            ((Entity)entity.Entity).DateUpdated = DateTime.Now;
                            ((Entity)entity.Entity).UpdatedBy = userId;
                            ((Entity)entity.Entity).UpdatedByName = name;
                        }
                    }
                }
            }
        }
    }
}
