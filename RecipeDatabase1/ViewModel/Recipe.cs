namespace RecipeDatabase1.ViewModel
{
	public class Recipe
	{
		public string Name { get; private set; } = "";
		public string Actions { get; private set; } = "";
		public string Ingredients { get; private set; } = "";
		public Recipe()
		{
		}
		public Recipe(string name, string actions, string ingredients)
		{
			Name = name;
			Actions = actions;			Ingredients = ingredients;
		}
	}
}
