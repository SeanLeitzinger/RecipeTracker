using System;

namespace RecipeTracker.Core.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
}
