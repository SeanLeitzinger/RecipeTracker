namespace RecipeTracker.Core.Models
{
    public class Direction : Entity
    {
        public int RecipeId { get; set; }

        public int Order { get; set; }
        public string Text { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
