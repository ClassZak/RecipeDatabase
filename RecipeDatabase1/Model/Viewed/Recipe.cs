namespace RecipeDatabase.Model.Viewed
{
	public class Recipe : DataBase.Recipe
	{
		public ViewModel.Ingredient? Ingredients { get; set; }
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
