using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecipeDatabase
{
	public class RecipeData
	{
		public List<string> Ingredients { get; set; }
		public string Actions { get; set; }

		public RecipeData()
		{
			Ingredients = new List<string>();
			Actions = "";
        }
		public string ShowIngredientsToString()
		{
			return string.Join("\n", Ingredients);
        }
    }
}
