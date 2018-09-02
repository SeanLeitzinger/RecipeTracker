namespace RecipeTracker.Core.Models
{
    public class Ingredient : Entity
    {
        public int RecipeId { get; set; }

        public string Description { get; set; }
        public int Order { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
