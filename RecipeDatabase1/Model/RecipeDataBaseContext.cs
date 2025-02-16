using Microsoft.EntityFrameworkCore;
using RecipeDatabase.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RecipeDatabase.Model
{
	class RecipeDataBaseContext : DbContext
	{
		private string _connectionString;

		public RecipeDataBaseContext(string connectionString)
		{
			_connectionString = connectionString;
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			foreach (var el in modelBuilder.Model.GetEntityTypes())
			{
				var name = el.GetTableName();
				if (name is null)
					continue;

				if (name[name.Length - 1] == 's')
					el.SetTableName(name.Substring(0, name.Length - 1));
			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}
		private async void RecipesDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
		{
			if (e.NewItem is Model.Viewed.Recipe newRecipe)
			{
				using (var context = new RecipeDataBaseContext(_connectionString))
				{
					var dbRecipe = new Model.DataBase.Recipe(newRecipe.Name, newRecipe.Actions);
					context.Recipe.Add(dbRecipe);
					await context.SaveChangesAsync();
					newRecipe.Id = dbRecipe.Id; // Обновляем ID после сохранения
				}
			}
		}

		public DbSet<Ingredient> Ingredient { get; set; }
		public DbSet<Recipe> Recipe { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
		public DbSet<MeasureUnit> MeasureUnit { get; set; }
	}
}
