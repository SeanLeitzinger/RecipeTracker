using System.Collections.Generic;

namespace RecipeTracker.Core.Models
{
    public class Recipe : Entity
    {
        public int Cook { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }
        public int Prep { get; set; }
        public int ReadyIn
        {
            get
            {
                return Cook + Prep;
            }
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Direction> Directions { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
