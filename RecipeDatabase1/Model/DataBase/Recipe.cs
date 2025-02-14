namespace RecipeDatabase.Model.DataBase
{
	public class Recipe : AModel
	{
		public string Name { get; set; }
		public string Actions { get; set; }
		public Recipe()
		{
		}

		public Recipe(Recipe recipe)
		{
			this.Id = recipe.Id;
			this.Name = recipe.Name;
			this.Actions = recipe.Actions;
		}
		public Recipe(string name, string actions)
		{
			Name = name;
			Actions = actions;
		}
	}
}
