namespace RecipeDatabase.Model.DataBase
{
	public class Ingredient : AModel
	{
		public string? Name { get; set; }
		public int? IdMeasureUnit { get; set; }
		public Ingredient()
		{
		}
		public Ingredient(Ingredient ingredient)
		{
			this.Id=ingredient.Id;
			Name = ingredient.Name;
			IdMeasureUnit = ingredient.IdMeasureUnit;
		}
		public Ingredient(string name, int? idMeasureUnit = null)
		{
			Name = name;
			IdMeasureUnit = idMeasureUnit;
		}
	}
}
