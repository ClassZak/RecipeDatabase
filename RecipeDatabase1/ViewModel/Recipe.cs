namespace RecipeDatabase1.ViewModel
{
	public class Recipe
	{
		private string _name;
		private string _actions;
		private string _ingredients;
		public string Name { get { return _name; } set {  } }
		public string Actions { get { return _actions; } set {  } }
		public string Ingredients { get { return _ingredients; } set {  } }
		public Recipe(string name, string actions, string ingredients)
		{
			_name = name;
			_actions = actions;			_ingredients = ingredients;
		}
	}
}
