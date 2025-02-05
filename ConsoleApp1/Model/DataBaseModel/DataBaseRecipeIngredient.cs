using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model.DataBaseModel
{
	public class DataBaseRecipeIngredient : DataBaseModel
	{
		private RecipeIngredient _recipeIngredient = new();
		public DataBaseRecipeIngredient(RecipeIngredient description)
		{
			_recipeIngredient = description;
		}
		public int IdRecipe { get { return _recipeIngredient.IdRecipe; } set { _recipeIngredient.IdRecipe = value; } }
		public int IdIngredient { get { return _recipeIngredient.IdIngredient; } set { _recipeIngredient.IdIngredient = value; } }
	}
}
