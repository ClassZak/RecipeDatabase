using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model.DataBaseModel
{
	public class DataBaseIngredient : DataBaseModel
	{
		private Ingredient _ingredient = new();
		public DataBaseIngredient(Ingredient ingredient)
		{
			_ingredient = ingredient;
		}
		public string? Name { get { return _ingredient.Name; } set { _ingredient.Name = value; } }
	}
}
