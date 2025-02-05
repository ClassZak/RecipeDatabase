using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase
{
	public class Recipe
	{
		public string Name { get; set; }
		public RecipeData Description { get; set; }



		public Recipe()
		{
			Description = new RecipeData();
        }
		public Recipe(string name, RecipeData description)
        {
            Name = name;
            Description = description;
        }

        public override bool  Equals(object obj)
		{
			if (obj is null)
				return false;

			if (!(obj is Recipe other))
				return false;

			return this.Name==other.Name && this.Description==other.Description;
		}
		public static bool operator==(Recipe recipe1, Recipe recipe2)
		{
			if (recipe1 is null || recipe2 is null)
				return false;

			return recipe1.Equals(recipe2);
		}
		public static bool operator!=(Recipe recipe1, Recipe recipe2)
		{
            if (recipe1 is null || recipe2 is null)
                return true;

            return !recipe1.Equals(recipe2);
		}

		public override int GetHashCode()
		{
			return Description.GetHashCode();
		}

        public override string ToString()
        {
            return Name+ string.Join("\n",Description.Ingredients)+ Description.Actions;
        }


		public bool EqualsNames(string name)
		{
			return this.Name == name;
		}
    }
}
