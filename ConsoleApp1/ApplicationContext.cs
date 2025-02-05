using ConsoleApp1.Model.DataBaseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class ApplicationContext : DbContext
	{
		protected readonly string _connectionString;
		public ApplicationContext(string connectionString)
		{
			_connectionString = connectionString;
			Database.EnsureCreated();
		}

		public DbSet<DataBaseRecipe> Recipe { get; set; }
		public DbSet<DataBaseIngredient> Ingredient { get; set; }
		public DbSet<DataBaseRecipeIngredient> RecipeIngredient { get; set; }


		public void AddNewIngredient(Model.Ingredient ingredient, bool addUnique = false)
		{
			bool allowAdding = false;
			{
				if (!addUnique)
					allowAdding = true;
				else
				{
					var ingredients = addUnique ? Ingredient.ToList() : [];
					if (ingredients.Find(x => x.Name == ingredient.Name) is not null)
						allowAdding = true;
				}
			}
			if (allowAdding)
				Ingredient.Add(new(ingredient));
		}
		public void AddNewRecipe(Model.Recipe recipe, List<int> ingredients)
		{
			if (ingredients.Count < 0)
				throw new Exception("Wrong recipe. No ingredients");

			Recipe.Add(new(recipe));

			foreach (var ingredient in ingredients)
				RecipeIngredient.Add(new(new Model.RecipeIngredient(Recipe.ToList().Last().Id, ingredient)));
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}
	}
}
