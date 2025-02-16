using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase.Model.Viewed
{
	public class Ingredient : DataBase.Ingredient
	{
		public Ingredient() : base()
		{
		}
		public Ingredient(DataBase.Ingredient ingredient):base(ingredient)
		{
		}
		public int IdRecipe { get; set; }
		public string? MeasureUnit{ get; set; }
		public int? Amount { get; set; }
		public Ingredient(string name, int idRecipe, int? idMeasureUnit=null, string? measureUnit=null, int? amount=null):
		base(name,idMeasureUnit)
		{
			MeasureUnit = measureUnit;
			Amount = amount;
			IdRecipe=idRecipe;
		}
	}
}
