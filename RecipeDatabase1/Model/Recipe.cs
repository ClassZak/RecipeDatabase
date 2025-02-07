namespace RecipeDatabase1.Model
{
	public class Recipe : AModel
	{
		public string? Name { get; set; }
		public string? Actions { get; set; }
		public Recipe()
		{
		}
		public Recipe(string name, string actions)
		{
			Name = name;
			Actions = actions;
		}
	}
}
