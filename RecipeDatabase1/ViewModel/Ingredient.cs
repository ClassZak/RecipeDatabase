using RecipeDatabase1.Model;

namespace RecipeDatabase1.ViewModel
{
	public class Ingredient
	{
		public string Name { get; private set; }
		public Ingredient(Ingredient ingredient)
		{
			Name = ingredient.Name!;
		}
		public Ingredient(string name)
		{
			Name = name;
		}


		public Ingredient(Model.Ingredient ingredient)
		{
			Name = ingredient.Name!;
		}
	}
}
