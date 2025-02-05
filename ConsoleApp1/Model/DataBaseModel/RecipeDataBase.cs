using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model.DBModel
{
	public class RecipeDataBase : DataBaseModel
	{
		private RecipeModel _recipe = new();
		public RecipeDataBase(RecipeModel recipe)
		{
			_recipe = recipe;
		}
		public string? Name { get { return _recipe.Name; } set { _recipe.Name = value; } }
		public string? Actions { get { return _recipe.Actions; } set { _recipe.Actions = value; } }
		public int IdDescription { get { return _recipe.IdDescription; } set { _recipe.IdDescription = value; } }
	}
}
