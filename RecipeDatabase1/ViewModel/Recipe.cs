namespace RecipeDatabase1.ViewModel
{
	public class Recipe : AViewModel
	{
		public string Name { get; private set; } = "";
		public string Actions { get; private set; } = "";
		public string Ingredients { get; private set; } = "";
		public Recipe()
		{
		}
		public Recipe(string name, string actions, string ingredients, int id=0) : base(id)
		{
			Name = name;
			Actions = actions;			Ingredients = ingredients;
		}
	}
}
