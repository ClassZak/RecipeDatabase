using System.Collections.ObjectModel;

namespace RecipeDatabase.Model.Viewed
{
	public class Recipe : DataBase.Recipe
	{
		public ObservableCollection<Model.Viewed.Ingredient> Ingredients { get; set; } = new();
		public Recipe() : base()
		{
		}

		public Recipe(Recipe recipe) : base(recipe)
		{
		}
		public Recipe(DataBase.Recipe recipe) : base(recipe)
		{
		}
		public Recipe(string name, string actions) : base(name, actions)
		{
		}
	}
}
