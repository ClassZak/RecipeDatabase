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
		public Ingredient(string name, int? idMeasureUnit = null) : base(name, idMeasureUnit)
		{
		}
	}
}
