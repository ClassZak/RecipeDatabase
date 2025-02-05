using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
	public class RecipeIngredient
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
