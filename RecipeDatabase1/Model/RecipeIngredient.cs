namespace RecipeDatabase1.Model
{
	public class RecipeIngredient : AModel
	{
		public int IdRecipe { get; set; }
		public int IdIngredient { get; set; }
		public RecipeIngredient()
		{
		}
		public RecipeIngredient(int idRecipe, int idIngredient)
		{
			IdRecipe = idRecipe;
			IdIngredient = idIngredient;
		}
	}
}
