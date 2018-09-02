namespace RecipeTracker.Core.Models
{
    public class Comment : Entity
    {
        public int RecipeId { get; set; }

        public string Text { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
