namespace RecipeDatabase1.Model
{
	public class Ingredient : AModel
	{
		public string? Name { get; set; }
		public Ingredient()
		{
		}
		public Ingredient(string name)
		{
			Name = name;
		}
	}
}
