namespace RecipeDatabase1.Model
{
	public class RecipeIngredient : AModel
	{
		public int IdRecipe { get; set; }
		public int IdIngredient { get; set; }
		public int? Amount { get; set; }
		public RecipeIngredient()
		{
		}
		public RecipeIngredient(int idRecipe, int idIngredient, int? amount=null)
		{
			IdRecipe = idRecipe;
			IdIngredient = idIngredient;
			Amount = amount;
		}
	}
}
