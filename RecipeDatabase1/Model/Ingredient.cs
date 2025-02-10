namespace RecipeDatabase1.Model
{
	public class Ingredient : AModel
	{
		public string? Name { get; set; }
		public int? IdMeasureUnit{ get; set; }
		public Ingredient()
		{
		}
		public Ingredient(string name, int? idMeasureUnit=null)
		{
			Name = name;
			IdMeasureUnit = idMeasureUnit;
		}
	}
}
