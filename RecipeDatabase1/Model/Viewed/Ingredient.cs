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

		public string? MeasureUnit{ get; set; }
		public int? Amount { get; set; }
		public Ingredient(string name, int? idMeasureUnit=null, string? measureUnit=null, int? Amount=null):
		base(name,idMeasureUnit)
		{
			MeasureUnit = measureUnit;
			Amount = Amount;
		}
	}
}
